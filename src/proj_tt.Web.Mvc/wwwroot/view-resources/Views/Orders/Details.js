$(function () {
    var _orderService = abp.services.app.order;
    var _$form = $('#updateStatusForm');

    _$form.on('submit', function (e) {
        e.preventDefault();

        var input = _$form.serializeFormToObject();
        input.Status = parseInt(input.Status);

        abp.message.confirm(
            'Are you sure you want to update the order status?',
            'Update Status',
            function (isConfirmed) {
                if (isConfirmed) {
                    _orderService.updateOrderStatus(input)
                        .done(function () {
                            abp.notify.info('Order status updated successfully');
                            location.reload();
                        });
                }
            }
        );
    });
}); 