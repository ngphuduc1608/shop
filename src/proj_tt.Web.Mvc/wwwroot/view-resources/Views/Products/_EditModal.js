(function ($) {
    var _productsService = abp.services.app.product,
        l = abp.localization.getSource('proj_tt'),
        _$modal = $('#editModal'),
        _$form = _$modal.find('form');


    function save() {

        if (!_$form.valid()) {
            return; // không submit nếu không hợp lệ
        }

        var formElement = _$form[0]; // DOM element
        var formData = new FormData(formElement); // Lấy toàn bộ form, bao gồm file

        console.log("Giá trị categoryId:", formData.get("CategoryId"));

        if (!formData.get('Discount')) {
            formData.set('Discount', 0);
        }
        abp.ui.setBusy(_$form);

        $.ajax({
            url: abp.appPath + 'Product/Update', // Đảm bảo bạn có controller hoặc endpoint này
            type: 'POST',
            data: formData,
            processData: false, // Không xử lý dữ liệu
            contentType: false, // Không đặt content-type mặc định
            success: function () {
                _$modal.modal('hide');
                abp.message.success(l('Sửa sản phẩm thành công '), 'Thành công');
                abp.event.trigger('product.edited');
            },
            error: function (err) {
                //abp.notify.error('Lỗi khi cập nhật sản phẩm!');
                abp.message.error(l('Sửa sản phẩm thất bại '), 'Thất bại');
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
                max: 2000000000000
            },
            Discount: {
                number: true,
                min: 0,
                max: 100
            },
            imagePreview: {
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
            imagePreview: {
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



})(jQuery)