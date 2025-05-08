(function ($) {
    var _productService = abp.services.app.product,
        l = abp.localization.getSource('proj_tt'),
        source = abp.localization.defaultSourceName;
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
        listAction: {
            ajaxFunction: _productService.getProductPaged,
            inputFilter: function () {
                var filter = $('#ProductsSearchForm').serializeFormToObject(true);

                // Get date range values
                var dateRange = $('#ProductionDateRange').val();
                if (dateRange) {
                    var dates = dateRange.split(' - ');
                    filter.startDate = dates[0];
                    filter.endDate = dates[1];
                }

                // Get price range values
                var minPrice = $('#MinPriceInput').val();
                var maxPrice = $('#MaxPriceInput').val();
                console.log('maxPrice:', maxPrice);
                console.log('minPrice:', minPrice);

                if (minPrice) filter.minPrice = minPrice;
                if (maxPrice) filter.maxPrice = maxPrice;

                // Get selected categories
                var selectedCategories = $('#CategoryDropdownEdit').val();
                console.log('selectedCategories:', selectedCategories);
                if (selectedCategories && selectedCategories.length > 0) {
                    filter.categoryIds = selectedCategories;
                }
                var dataTable = _$table.DataTable();
                var order = dataTable.order(); // ví dụ: [[0, 'asc']]
                if (order.length > 0) {
                    var columnIndex = order[0][0];
                    var direction = order[0][1]; // 'asc'/ 'desc'
                    var sortField = dataTable.column(columnIndex).dataSrc(); // lay ten data cot set ở columnDefs

                    filter.sorting = sortField + ' ' + direction;
                }

                console.log('Filter data:', filter);
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
                orderable: true,

            },
            {
                targets: 2,
                data: 'price',
                orderable: true,
                render: function (data, type, row, meta) {
                    if (!data) return '0';
                    return Number(data) + ' VND';
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
                sortable: false,
                render: function (data, type, row, meta) {
                    if (!data) return '';
                    return `<img src="${data}" alt="image" style="width: 60px; height: 60px; border-radius: 8px; object-fit: cover;" />`;
                }
            },
            {
                targets: 5,
                data: 'nameCategory',
                sortable: false,
            },
            {
                targets: 6,
                data: 'stock',
                sortable: false,

            },
            {
                targets: 7,
                data: 'productionDate',
                orderable: true,
                render: function (data, type, row, meta) {
                    if (!data) return '';
                    const date = new Date(data);
                    return date.toLocaleDateString('vi-VN', { year: 'numeric', month: '2-digit', day: '2-digit' }).replace(/(\d{2})\/(\d{2})\/(\d{4})/, '$3/$2/$1');
                }
            },
            {
                targets: 8,
                data: 'creationTime',
                orderable: true,
                render: function (data, type, row, meta) {
                    if (!data) return '';
                    const date = new Date(data);
                    return date.toLocaleDateString('vi-VN', { year: 'numeric', month: '2-digit', day: '2-digit' }).replace(/(\d{2})\/(\d{2})\/(\d{4})/, '$3/$2/$1');
                }
            },
            {
                targets: 9,
                data: 'lastModificationTime',
                orderable: true,
                render: function (data, type, row, meta) {
                    if (!data) return '';
                    const date = new Date(data);
                    return date.toLocaleDateString('vi-VN', { year: 'numeric', month: '2-digit', day: '2-digit' }).replace(/(\d{2})\/(\d{2})\/(\d{4})/, '$3/$2/$1');
                }
            },
            {
                targets: 10,
                data: null,
                sortable: false,
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


    // Preview ảnh 
    $('#createModal #image').on('change', function (event) { 
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#createModal #imagePreview').attr('src', e.target.result).show(); // lấy result gắn vào src
        };
        //console.log('reader', reader);

        reader.readAsDataURL(this.files[0]); //chuyển sang dạng base64 và gắn vào src
    });

    // Reset preview ảnh 
    $('#createModal').on('hidden.bs.modal', function () { // sự kiện của bootstrap khi đóng modal
        $('#createModal #imagePreview').attr('src', '#').hide(); 
        $('#createModal #image').val('');
    });

    flatpickr("#createModal #SelectedDate", {
        enableTime: false,
        dateFormat: "Y-m-d",
        locale: "vi" // hoặc "default" nếu không cần tiếng Việt
    });

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

    // Initialize date range picker
    $('#ProductionDateRange').daterangepicker({
        locale: {
            format: 'YYYY/MM/DD',
            separator: ' - ',
            applyLabel: 'Áp dụng',
            cancelLabel: 'Hủy',
            fromLabel: 'Từ',
            toLabel: 'Đến',
            customRangeLabel: 'Tùy chọn',
            daysOfWeek: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
            monthNames: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
            firstDay: 1
        },
        autoUpdateInput: false
    });

    $('#ProductionDateRange').on('apply.daterangepicker', function(ev, picker) {
        $(this).val(picker.startDate.format('YYYY/MM/DD') + ' - ' + picker.endDate.format('YYYY/MM/DD'));
    });

    $('#ProductionDateRange').on('cancel.daterangepicker', function(ev, picker) {
        $(this).val('');
    });

    // Initialize price range slider
    $("#priceRange").ionRangeSlider({
        type: "double",
        min: 0,
        max: 50000000,
        from: 0,
        to: 50000000,
        step: 1000,
        grid: true,
        prefix: "",
        postfix: " VND",
        onFinish: function (data) {
            $("#MinPriceInput").val(data.from);
            $("#MaxPriceInput").val(data.to);
        }
    });

    // Update slider when input values change
    $("#MinPriceInput").on('change', function() {
        var value = parseInt($(this).val()) || 0;
        $("#priceRange").data("ionRangeSlider").update({
            from: value
        });
    });

    $("#MaxPriceInput").on('change', function() {
        var value = parseInt($(this).val()) || 50000000;
        var slider = $("#priceRange").data("ionRangeSlider");
        
        // If the input value is greater than current max, update the max
        if (value > slider.options.max) {
            slider.update({
                max: value,
                to: value
            });
        } else {
            slider.update({
                to: value
            });
        }
    });

    // Initialize select2 for categories
    $('#CategoryDropdownEdit').select2({
        placeholder: "Chọn danh mục",
        allowClear: true,
        width: '100%'
    });

    // Clear filter button
    $('.btn-clear').on('click', function() {
        $('#ProductsSearchForm')[0].reset();
        $('#ProductionDateRange').val('');
        $('#CategoryDropdownEdit').val(null).trigger('change');
        $("#priceRange").data("ionRangeSlider").reset();
        _$productsTable.ajax.reload();
    });

})(jQuery);