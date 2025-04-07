(function ($) {
    var _productsService = abp.services.app.product,
        l = abp.localization.getSource('proj_tt'),
        _$modal = $('#editModal'),
        _$form = _$modal.find('form');


    //function save() {
    //    //if (!_$form.valid()) {
    //    //    return;
    //    //}


    //    var products = _$form.serializeFormToObject();


    //    abp.ui.setBusy(_$form);
    //    _productsService.update(products).done(function () {
    //        _$modal.modal('hide');
    //        abp.notify.info(l('SavedSuccessfully'));
    //        abp.event.trigger('product.edited', products);
    //    }).always(function () {
    //        abp.ui.clearBusy(_$form);
    //    });
    //}

    function save() {
        var formElement = _$form[0]; // DOM element
        var formData = new FormData(formElement); // Lấy toàn bộ form, bao gồm file

        console.log("Giá trị categoryId:", formData.get("CategoryId"));


        abp.ui.setBusy(_$form);

        $.ajax({
            url: abp.appPath + 'Product/Update', // Đảm bảo bạn có controller hoặc endpoint này
            type: 'POST',
            data: formData,
            processData: false, // Không xử lý dữ liệu
            contentType: false, // Không đặt content-type mặc định
            success: function () {
                _$modal.modal('hide');
                abp.notify.info(l('SavedSuccessfully'));
                abp.event.trigger('product.edited');
            },
            error: function (err) {
                abp.notify.error('Lỗi khi cập nhật sản phẩm!');
                console.error(err);
            },
            complete: function () {
                abp.ui.clearBusy(_$form);
            }
        });
    }



    _$form.closest('div.modal-content').find(".save-button").click(function (e) {
        e.preventDefault();
        save();
    });





    //// xem trước ảnh trên web
    //$(document).ready(function () {
    //    // Lắng nghe sự kiện thay đổi của input file
    //    $("#image").change(function (event) {
    //        var reader = new FileReader();

    //        // Khi file được tải lên
    //        reader.onload = function (e) {
    //            // Lấy src của ảnh đã chọn
    //            $("#imagePreview").attr("src", e.target.result);

    //            // Hiển thị ảnh
    //            $("#imagePreview").show();
    //        };

    //        // Đọc ảnh đã chọn
    //        reader.readAsDataURL(this.files[0]);
    //    });
    //    //reset ảnh khi out modal
    //    $('#editModal').on('hidden.bs.modal', function () {
    //        // Reset lại ảnh preview và ẩn nó đi
    //        $("#imagePreview").attr("src", "#");
    //        $("#imagePreview").hide();

    //        // Reset lại input file
    //        $("#image").val('');
    //    });
    //});



})(jQuery)