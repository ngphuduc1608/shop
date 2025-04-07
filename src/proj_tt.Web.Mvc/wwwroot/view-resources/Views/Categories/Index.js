(function ($) {
    var _categoriesService = abp.services.app.categories,
        l = abp.localization.getSource('proj_tt'),
        _$createModal = $('#createModal'),

        _$createForm = _$createModal.find('form'),
        _$table = $('#CategoriesTable');

    var _$categoriesTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _categoriesService.getAllCategories,
            inputFilter: function () {
                //return $('#ProductsSearchForm').serializeFormToObject(true);
                var filter = $('#CategoriesSearchForm').serializeFormToObject(true);
                console.log('Dữ liệu gửi đi:', filter);  // Kiểm tra giá trị filter
                return filter;
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$categoriesTable.draw(false)
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
                data: 'nameCategory',
                sortable: false
            },
            {
                targets: 2,
                data: 'creationTime',
                sortable: false,
            },
            {
                targets: 3,
                data: 'lastModificationTime',
                sortable: false,
            },
            {
                targets: 4,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-primary edit-category" data-category-id="${row.id}" data-toggle="modal" data-target="#editModal">`,
                        `   <i class="fas fa-edit"></i>`,
                        '   </button>',
                        `   <button type="button" class="btn btn-danger delete-category" data-category-id="${row.id}" data-category-name="${row.nameCategory}">`,
                        `       <i class="fas fa-trash"></i>`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    _$createForm.find('.save-button').on('click', (e) => {
        e.preventDefault();
        
        var category = _$createForm.serializeFormToObject();

        abp.ui.setBusy(_$createModal);
        _categoriesService.create(category).done(function () {
            _$createModal.modal('hide');
            _$createForm[0].reset();
            abp.notify.info(l('SaveSucessFully'));
            _$categoriesTable.ajax.reload();

        }).always(function () {
            abp.ui.clearBusy(_$createModal);
        });
    });


    //$(document).on('click','a[data-ta]')

    $(document).on('click', '.edit-category', function (e) {
        var categoryId = $(this).attr('data-category-id');

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Categories/EditModal?categoryId=' + categoryId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#editModal div.modal-content').html(content);
            },
            error: function (e) {

            }
        });
    });


    abp.event.on('category.edited', (data) => {
        _$categoriesTable.ajax.reload();
    });


    $(document).on('click', '.delete-category', function () {
        var categoryId = $(this).attr('data-category-id');
        var categoryName = $(this).attr('data-category-name');

        deleteCategory(categoryId, categoryName);


    });

    function deleteCategory(categoryId, categoryName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                categoryName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _categoriesService.delete(categoryId).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$categoriesTable.ajax.reload();
                    });
                }
            }
        );
    }


    

    $('.btn-search').on('click', (e) => {
        _$categoriesTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$categoriesTable.ajax.reload();
            return false;
        }
    });


})(jQuery);
