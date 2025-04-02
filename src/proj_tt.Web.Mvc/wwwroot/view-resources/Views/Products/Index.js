(function ($) {
    var _productService = abp.services.app.product,
        l = abp.localization.getSource('proj_tt'),
        _$modal = $('#editModal'),
        _$deletemodal = $('#deleteModal'),

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
                data: 'creationTime',
                sortable: false,
            },
            {
                targets: 6,
                data: 'lastModificationTime',
                sortable: false,
            },
            {
                targets: 7,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button class="btn btn-primary" data-toggle="modal" data-target="#editModal">`,
                        `   <i class="fas fa-edit"></i>`,
                        '   </button>',
                        `   <button class="btn btn-danger" data-toggle="modal" data-target="#deleteModal" style="">`,
                        `       <i class="fas fa-trash"></i>`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });
    // xem trước ảnh trên web
    $(document).ready(function () {
        // Lắng nghe sự kiện thay đổi của input file
        $("#image").change(function (event) {
            var reader = new FileReader();

            // Khi file được tải lên
            reader.onload = function (e) {
                // Lấy src của ảnh đã chọn
                $("#imagePreview").attr("src", e.target.result);

                // Hiển thị ảnh
                $("#imagePreview").show();
            };

            // Đọc ảnh đã chọn
            reader.readAsDataURL(this.files[0]);
        });
        //reset ảnh khi out modal
        $('#createModal').on('hidden.bs.modal', function () {
            // Reset lại ảnh preview và ẩn nó đi
            $("#imagePreview").attr("src", "#");
            $("#imagePreview").hide();

            // Reset lại input file
            $("#image").val('');
        });
    });

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
