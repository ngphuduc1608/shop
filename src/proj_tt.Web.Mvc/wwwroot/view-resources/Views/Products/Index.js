(function ($) {
    var _productService = abp.services.app.product,
        l = abp.localization.getSource('proj_tt'),
        _$modal = $('#createModal'),

        _$form = _$modal.find('form'),
        _$table = $('#ProductsTable');

    
    var _permissions = {
        create: abp.auth.isGranted('Pages.Products.Create'),
        edit: abp.auth.isGranted('Pages.Products.Edit'),
        delete: abp.auth.isGranted('Pages.Products.Delete')
    };


    var _$productsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        ordering: true,
        processing: true,
        order: [[6, 'desc']],
        //listAction: {
        //    ajaxFunction: _productService.getProductPaged,
        //    inputFilter: function () {
        //        //return $('#ProductsSearchForm').serializeFormToObject(true);
        //        var filter = $('#ProductsSearchForm').serializeFormToObject(true);
        //        console.log('Dữ liệu gửi đi:', filter);  // Kiểm tra giá trị filter
        //        return filter;
        //    }
        //},
        ajax: function (data, callback, settings) {
            var filter = $('#ProductsSearchForm').serializeFormToObject(true);
            console.log('Filter:', filter);
            console.log('DataTables params:', data);
            console.log('settings params:', settings);


            var sortColumn = data.columns[data.order[0].column].data;
            var sortDirection = data.order[0].dir;
            console.log('sortColumn params:', sortColumn);
            console.log('sortDirection params:', sortDirection);



            _productService.getProductPaged($.extend({}, filter, {
                skipCount: data.start,                // Phân trang: bắt đầu từ đâu
                maxResultCount: data.length,          // Phân trang: bao nhiêu dòng
                sorting: sortColumn + " " + sortDirection
            }))
                .done(function (result) {
                    callback({
                        recordsTotal: result.totalCount,
                        recordsFiltered: result.totalCount,
                        data: result.items
                    });
                });
        },

        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$productsTable.draw(false)
            },
        ],
        responsive: {
            details: {
                type: 'column'
            }
        },
        columnDefs: [
            {
                targets: 0,
                className: 'control',
                defaultContent: '',
                orderable: false
            },
            {
                targets: 1,
                data: 'name',
                orderable: true,

            },
            {
                targets: 2,
                data: 'price',
                orderable: true,
                render: function (data, type, row, meta) {
                    if (!data) return '0';
                    return Number(data).toLocaleString("vi-VN") + ' VND';
                }
            },
            {
                targets: 3,
                data: 'discount',
                orderable: true,
            },
            {
                targets: 4,
                data: 'imageUrl',
                orderable: false,
                render: function (data, type, row, meta) {
                    if (!data) return '';
                    return `<img src="${data}" alt="image" style="width: 60px; height: 60px; border-radius: 8px; object-fit: cover;" />`;
                }
            },
            {
                targets: 5,
                data: 'nameCategory',
                orderable: false,
            },
            {
                targets: 6,
                data: 'creationTime',
                orderable: true,
                render: function (data, type, row, meta) {
                    if (!data) return '';
                    const date = new Date(data);
                    return date.toLocaleString('vi-VN');
                }
            },
            {
                targets: 7,
                data: 'lastModificationTime',
                orderable: true,
                render: function (data, type, row, meta) {
                    if (!data) return '';
                    const date = new Date(data);
                    return date.toLocaleString('vi-VN');
                }
            },
            {
                targets: 8,
                data: null,
                orderable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => { // data: giá trị, type: kiểu xử lý , row là toàn bộ dữ liêu của hàng đó , meta là vị trị của ô đó  
                    return [
                        `<div class="dropdown">
                            <button class="btn btn-sm btn-primary dropdown-toggle" type="button" id="actionDropdown_${row.id}" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                 ${l('Actions')}
                            </button>
                            <div class="dropdown-menu p-0" aria-labelledby="actionDropdown_${row.id}">
                                <button type="button" class="dropdown-item text-secondary edit-product" data-product-id="${row.id}" data-toggle="modal" data-target="#editModal">
                                    <i class="fas fa-edit mr-2"></i>  ${l('Edit')}
                                </button>
                                <div class="dropdown-divider m-0"></div>
                                <button type="button" class="dropdown-item text-danger delete-product" data-product-id="${row.id}" data-product-name="${row.name}" data-toggle="modal" data-target="#deleteModal">
                                    <i class="fas fa-trash mr-2"></i>  ${l('Delete')}
                                </button>
                            </div>
                        </div>`
                    ];
                }
            }
        ]
    });




    _$form.validate({
        rules: {
            Name: {
                required: true,
                minlength: 3,
                maxlength: 100
            },
            Price: {
                required: true,
                number: true,
                min: 0,
                max:2000000000000
            },
            Discount: {
                number: true,
                min: 0,
                max: 100
            },
            ImageUrl: {
                required: true,
                imageExtension: true,
                filesize: 2 * 1024 * 1024 
            },
            CategoryId: {
                required: true
            }
        },
        messages: {
            Name: {
                required: "Tên sản phẩm không được để trống",
                minlength: l("PleaseEnterAtLeastNCharacter") ,
                maxlength: l("PleaseEnterNoMoreThanNCharacter")
            },
            Price: {
                required: "Vui lòng nhập giá",
                number: "Giá phải là số",
                min: "Giá phải lớn hơn hoặc bằng 0",
                max: "Max là 2000 tỷ thôi bro 😒",
            },
            Discount: {
                number: "Giảm giá phải là số",
                min: "Tối thiểu là 0%",
                max: "Tối đa là 100%"
            },
            ImageUrl: {
                required: "Vui lòng chọn ảnh",
                imageExtension: "Chỉ chấp nhận file ảnh JPG, PNG, GIF, BMP",
                filesize: "Dung lượng ảnh tối đa là 2MB"
            },
            CategoryId: {
                required: "Vui lòng chọn danh mục"
            }
        }
    });

    // Thêm phương thức kiểm tra size ảnh
    $.validator.addMethod('filesize', function (value, element, param) {
        return this.optional(element) || (element.files[0].size <= param);
    }, 'Dung lượng ảnh vượt quá giới hạn');

    $.validator.addMethod("imageExtension", function (value, element) {
        if (element.files.length === 0) return false;
        var fileName = element.files[0].name;
        return /\.(jpe?g|png|gif|bmp|webp)$/i.test(fileName);
    }, "Chỉ chấp nhận ảnh định dạng JPG, PNG, GIF, BMP");


    _$form.find('.save-button').on('click', (e) => {

        //if (!_permissions.create) {
        //    abp.message.warn("Bạn không đủ quyền để thêm sản phẩm!");
        //    return;
        //}
        e.preventDefault(); // submit không reload trang


        if (!_$form.valid()) {
            return; // không submit nếu không hợp lệ
        }


        var formElement = _$form[0];
        var formData = new FormData(formElement); // lấy cả input và ảnh
        console.log('discount', formData.get('Discount'));

        if (!formData.get('Discount')) {
            formData.set('Discount', 0);
        }

        abp.ui.setBusy(_$modal); // hiển thị trạng thái loading 

        $.ajax({
            url: abp.appPath + 'Product/Create', // Controller Create
            type: 'POST',
            data: formData,
            processData: false, // không chuyển data thành chuỗi Jquer
            contentType: false, //để jQuery không đặt header Content-Type
            success: function () {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.message.success(l('SuccessfullyRegistered'), l('Success'));
                _$productsTable.ajax.reload();
            },
            error: function (err) {
                abp.notify.error("Thêm sản phẩm thất bại!");
                console.error(err);
            },
            complete: function () {
                abp.ui.clearBusy(_$modal); // khi hoàn tất , Tắt trạng thái loading dù thành công hay thất bại.
            }
        });
    });


    function ImagePreview(modalSelector) {
        const $modal = $(modalSelector);

        // Preview ảnh 
        $modal.find('#image').on('change', function () {
            var reader = new FileReader();

            reader.onload = function (e) {
                $modal.find('#imagePreview').attr('src', e.target.result).show(); // lấy result gắn vào src
            };
            //console.log('reader', reader);

            reader.readAsDataURL(this.files[0]); //chuyển sang dạng base64 và gắn vào src
        });

        // Reset preview ảnh 
        $modal.on('hidden.bs.modal', function () { // sự kiện của bootstrap khi đóng modal
            $modal.find('#imagePreview').attr('src', '#').hide();
            $modal.find('#image').val('');
        });
    }

    ImagePreview('#createModal');
 



    $(document).on('click', '.edit-product', function (e) {
        

        e.preventDefault();
        if (!_permissions.edit) {
            abp.message.warn("Bạn không đủ quyền để chỉnh sửa sản phẩm!");
            $('#editModal').modal('hide');  // Đóng modal
            $('.modal-backdrop').remove();  // Loại bỏ lớp phủ

            $('body').removeClass('modal-open');  // Loại bỏ lớp modal-open
            $('body').css('padding-right', '');   // Gỡ bỏ padding-right nếu có

            return;
        }
        var productId = $(this).attr('data-product-id');
        console.log('productId ', productId);
        




        abp.ajax({
            url: abp.appPath + 'Product/EditModal?productId=' + productId,  // gọi EditModal trong ProductController và truyền productId
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                //console.log('content:', content); 
                $('#editModal div.modal-content').html(content); // add cái form của editmodal vào index

                ImagePreview('#editModal');

            },
            error: function (e) {
                
            }
        });
    });


    abp.event.on('product.edited', (data) => {
        _$productsTable.ajax.reload();
    });



    $(document).on('click', '.delete-product', function () {

        if (!_permissions.delete) {
            abp.message.warn("Bạn không đủ quyền để xoá sản phẩm!");
            return;
        }

        var productId = $(this).attr('data-product-id');
        var productName = $(this).attr('data-product-name');
        deleteProduct(productId, productName);


    });

    function deleteProduct(productId, productName) {
        abp.message.confirm(           // confirm(message,title,callback)
            abp.utils.formatString( // chèn productName vào nội dung confirm
                l('AreYouSureWantToDelete'),
                productName),
            "Xác nhận xóa sản phẩm",
            (isConfirmed) => {
                if (isConfirmed) {
                    _productService.delete(productId).done(() => {
                        abp.message.success(l('SuccessfullyDeleted'), l('Success'));
                        _$productsTable.ajax.reload();
                    });
                }
            }
        );
    }


    $('.btn-search').on('click', (e) => {
        _$productsTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$productsTable.ajax.reload();
            return false;
        }
    });


})(jQuery);
