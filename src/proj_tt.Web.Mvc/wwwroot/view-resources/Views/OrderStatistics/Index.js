$(function () {
    'use strict';

    // Initialize variables
    var orderStatusChart = null;
    var currentPeriod = 'day';

    // Load order counts by period
    function loadOrderCounts(period) {
        $.ajax({
            url: '/OrderStatistics/GetOrderCountsByPeriod',
            type: 'GET',
            data: { period: period },
            success: function (response) {
                if (response.success && response.result) {
                    var data = response.result;
                    // Update summary cards
                    $('#totalOrders').text(data.totalOrders);
                    $('#pendingOrders').text(data.pendingOrders);
                    $('#completedOrders').text(data.completedOrders);
                    $('#cancelledOrders').text(data.cancelledOrders);

                    // Update success rate
                    var successRate = Math.round(data.successRate);
                    $('#successRate').text(successRate);
                    $('#successRateBar').css('width', successRate + '%');
                } else {
                    console.error('Invalid response format:', response);
                }
            },
            error: function(xhr, status, error) {
                console.error('Error loading order counts:', error);
            }
        });
    }

    // Load order status distribution
    function loadOrderStatusDistribution() {
        $.ajax({
            url: '/OrderStatistics/GetOrderStatusDistribution',
            type: 'GET',
            success: function (response) {
                if (!response.success || !response.result) {
                    console.error('Invalid response format:', response);
                    return;
                }

                var data = response.result;
                if (orderStatusChart) {
                    orderStatusChart.destroy();
                }

                // Convert data to arrays for Chart.js
                var labels = [];
                var counts = [];
                var percentages = [];

                if (data && data.length > 0) {
                    data.forEach(function(item) {
                        labels.push(item.status);
                        counts.push(item.count);
                        percentages.push(item.percentage);
                    });
                }

                var ctx = document.getElementById('orderStatusChart').getContext('2d');
                orderStatusChart = new Chart(ctx, {
                    type: 'doughnut',
                    data: {
                        labels: labels,
                        datasets: [{
                            data: counts,
                            backgroundColor: [
                                'rgb(255, 99, 132)',   // Cancelled
                                'rgb(54, 162, 235)',  // Pending
                                'rgb(75, 192, 192)'   // Completed
                            ]
                        }]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: {
                            legend: {
                                position: 'bottom'
                            },
                            tooltip: {
                                callbacks: {
                                    label: function(context) {
                                        var label = context.label || '';
                                        var value = context.raw || 0;
                                        var percentage = percentages[context.dataIndex];
                                        return label + ': ' + value + ' (' + percentage.toFixed(1) + '%)';
                                    }
                                }
                            }
                        }
                    }
                });
            }
        });
    }

    // Handle period button clicks
    $('.period-btn').click(function() {
        $('.period-btn').removeClass('active');
        $(this).addClass('active');
        currentPeriod = $(this).data('period');
        loadOrderCounts(currentPeriod);
    });

    // Initial load
    loadOrderCounts(currentPeriod);
    loadOrderStatusDistribution();

    // Refresh data every 5 minutes
    setInterval(function () {
        loadOrderCounts(currentPeriod);
        loadOrderStatusDistribution();
    }, 300000);
}); 