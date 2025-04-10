(function ($) {
    var _productService = abp.services.app.product,
        l = abp.localization.getSource('proj_tt'),
        _$modal = $('#createModal'),

        _$form = _$modal.find('form'),
        _$table = $('#ProductsTable');

    var _$productsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _productService.getProductPaged,
            inputFilter: function () {
                //return $('#ProductsSearchForm').serializeFormToObject(true);
                var filter = $('#ProductsSearchForm').serializeFormToObject(true);
                console.log('Dữ liệu gửi đi:', filter);  // Kiểm tra giá trị filter
                return filter;
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$productsTable.draw(false)
            }
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
            },
            {
                targets: 1,
                data: 'name',
                sortable: false,
                title: 'Tên sản phẩm',

            },
            {
                targets: 2,
                data: 'price',
                sortable: false,
                title: 'Giá',
                render: function (data, type, row, meta) {
                    if (!data) return '0';
                    return Number(data).toLocaleString('vi-VN') + ' đ';
                }
            },
            {
                targets: 3,
                data: 'discount',
                sortable: false,
            },
            {
                targets: 4,
                data: 'imageUrl',
                sortable: false,
                title: 'Hình ảnh sản phẩm',
                render: function (data, type, row, meta) {
                    if (!data) return '';
                    return `<img src="${data}" alt="image" style="width: 60px; height: 60px; border-radius: 8px; object-fit: cover;" />`;
                }
            },
            {
                targets: 5,
                data: 'nameCategory',
                sortable: false,
                title: 'Tên danh mục',
            },
            {
                targets: 6,
                data: 'creationTime',
                sortable: false,
                title: 'Thời gian tạo',
                render: function (data, type, row, meta) {
                    if (!data) return '';
                    const date = new Date(data);
                    return date.toLocaleString('vi-VN');
                }
            },
            {
                targets: 7,
                data: 'lastModificationTime',
                sortable: false,
                title: 'Thời gian sửa gần nhất ',
                render: function (data, type, row, meta) {
                    if (!data) return '';
                    const date = new Date(data);
                    return date.toLocaleString('vi-VN');
                }
            },
            {
                targets: 8,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => { // data: giá trị, type: kiểu xử lý , row là toàn bộ dữ liêu của hàng đó , meta là vị trị của ô đó  
                    return [
                        `<div class="dropdown">
                            <button class="btn btn-sm btn-primary dropdown-toggle" type="button" id="actionDropdown_${row.id}" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                Hành động
                            </button>
                            <div class="dropdown-menu p-0" aria-labelledby="actionDropdown_${row.id}">
                                <button type="button" class="dropdown-item text-secondary edit-product" data-product-id="${row.id}" data-toggle="modal" data-target="#editModal">
                                    <i class="fas fa-edit mr-2"></i> Sửa
                                </button>
                                <div class="dropdown-divider m-0"></div>
                                <button type="button" class="dropdown-item text-danger delete-product" data-product-id="${row.id}" data-product-name="${row.name}" data-toggle="modal" data-target="#deleteModal">
                                    <i class="fas fa-trash mr-2"></i> Xóa
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
            }
        },
        messages: {
            Name: {
                required: "Tên sản phẩm không được để trống",
                minlength: "Tên ít nhất 3 ký tự",
                maxlength: "Tên tối đa 100 ký tự"
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
        e.preventDefault(); // submit không reload trang

        if (!_$form.valid()) {
            return; // không submit nếu không hợp lệ
        }


        var formElement = _$form[0];
        var formData = new FormData(formElement); // lấy cả input và ảnh
        console.log('discount', formData.get('discount'));

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
                abp.message.success(l('Tạo sản phẩm thành công '), 'Thành công');
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


    // Preview ảnh khi chọn ảnh trong createModal
    $('#createModal #image').on('change', function (event) { 
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#createModal #imagePreview').attr('src', e.target.result).show();
        };

        reader.readAsDataURL(this.files[0]); //chuyển sang dạng base64 và gắn vào src
    });

    // Reset preview ảnh khi đóng modal create
    $('#createModal').on('hidden.bs.modal', function () {
        $('#createModal #imagePreview').attr('src', '#').hide(); // gắn ảnh bằng #
        $('#createModal #image').val('');
    });



    $(document).on('click', '.edit-product', function (e) {
        var productId = $(this).attr('data-product-id');
        console.log('productId ', productId);

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Product/EditModal?productId=' + productId,  // gọi EditModal trong ProductController và truyền productId
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                //console.log('content:', content); 
                $('#editModal div.modal-content').html(content); // add cái form của editmodal vào index

                // Thêm đoạn xử lý ảnh ở đây cho editModal
                $('#editModal #image').on('change', function (event) {
                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#editModal #imagePreview').attr('src', e.target.result).show();
                    };

                    reader.readAsDataURL(this.files[0]);
                });

                // Reset ảnh khi đóng modal
                $('#editModal').on('hidden.bs.modal', function () {
                    $('#editModal #imagePreview').attr('src', '#').hide();
                    $('#editModal #image').val('');
                });
            },
            error: function (e) {

            }
        });
    });


    abp.event.on('product.edited', (data) => {
        _$productsTable.ajax.reload();
    });



    $(document).on('click', '.delete-product', function () {
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
                        abp.message.success(l('SuccessfullyDeleted'), 'Thành công');
                        _$productsTable.ajax.reload();
                    }).fail((error) => {
                        let errorMessage = "Đã xảy ra lỗi khi xóa!";

                        if (error.responseJSON && error.responseJSON.error && error.responseJSON.error.message) {
                            errorMessage = error.responseJSON.error.message;
                        }

                        abp.message.error(errorMessage, "Lỗi");
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
