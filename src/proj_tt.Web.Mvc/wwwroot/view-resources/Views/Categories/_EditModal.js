(function ($) {
    var _categoriesService = abp.services.app.categories,
        l = abp.localization.getSource('proj_tt'),
        _$editModal = $('#editModal'),
        _$editForm = _$editModal.find('form');


    function save() {
        //if (!_$form.valid()) {
        //    return;
        //}


        var categories = _$editForm.serializeFormToObject();
        console.log('Dữ liệu categories gửi đi :', categories);  // Kiểm tra giá trị filter


        abp.ui.setBusy(_$editForm);
        _categoriesService.update(categories).done(function () {
            _$editModal.modal('hide');
            abp.notify.info(l('SavedSuccessfully'));
            abp.event.trigger('category.edited', categories);
        }).always(function () {
            abp.ui.clearBusy(_$editForm);
        });
    }


    _$editForm.closest('div.modal-content').find(".save-button").click(function (e) {
        e.preventDefault();
        save();
    });


})(jQuery)