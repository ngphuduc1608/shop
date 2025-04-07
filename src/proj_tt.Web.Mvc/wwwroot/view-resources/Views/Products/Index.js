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
                sortable: false
            },
            {
                targets: 2,
                data: 'price',
                sortable: false
            },
            {
                targets: 3,
                data: 'discount',
                sortable: false
            },
            {
                targets: 4,
                data: 'imageUrl',
                sortable: false,
            },
            {
                targets: 5,
                data: 'nameCategory',
                sortable: false,
            },
            {
                targets: 6,
                data: 'creationTime',
                sortable: false,
            },
            {
                targets: 7,
                data: 'lastModificationTime',
                sortable: false,
            },
            {
                targets: 8,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-primary edit-product" data-product-id="${row.id}" data-toggle="modal" data-target="#editModal">`,
                        `   <i class="fas fa-edit"></i>`,
                        '   </button>',
                        `   <button type="button" class="btn btn-danger delete-product" data-product-id="${row.id}" data-product-name="${row.name}" data-toggle="modal" data-target="#deleteModal">`,
                        `       <i class="fas fa-trash"></i>`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });



    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();

        var formElement = _$form[0];
        var formData = new FormData(formElement); // lấy cả input và ảnh

        abp.ui.setBusy(_$modal);

        $.ajax({
            url: abp.appPath + 'Product/Create', // Controller Create
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function () {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.notify.info(l('SaveSucessFully'));
                _$productsTable.ajax.reload();
            },
            error: function (err) {
                abp.notify.error("Thêm sản phẩm thất bại!");
                console.error(err);
            },
            complete: function () {
                abp.ui.clearBusy(_$modal);
            }
        });
    });


    // Preview ảnh khi chọn ảnh trong createModal
    $('#createModal #image').on('change', function (event) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#createModal #imagePreview').attr('src', e.target.result).show();
        };

        reader.readAsDataURL(this.files[0]);
    });

    // Reset preview ảnh khi đóng modal create
    $('#createModal').on('hidden.bs.modal', function () {
        $('#createModal #imagePreview').attr('src', '#').hide();
        $('#createModal #image').val('');
    });



    $(document).on('click', '.edit-product', function (e) {
        var productId = $(this).attr('data-product-id');

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Product/EditModal?productId=' + productId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#editModal div.modal-content').html(content);

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
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                productName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _productService.delete(productId).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
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
