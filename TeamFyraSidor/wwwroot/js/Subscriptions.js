$(document).ready(function () {
    loadPlans();
    onProductChange();
    onPlanChange();
});

function loadPlans() {
    var url = '/User/Subscription?handler=Products';
    var productId = $('.subscription-products').val();
    var data = { "productId": productId };
    var price = $('.subscription-prices');
    price.empty();
    $.getJSON(url, data, function (productPrice) {
        if (productPrice !== null) {
            $.each(productPrice, function (index, htmlData) {
                price.append($('<option />', {
                    value: htmlData.value,
                    text: htmlData.text
                }));
            });
            loadPlanPrice();
        }
    });
}

function loadPlanPrice() {
    var url = '/User/Subscription?handler=Price'
    var priceId = $('.subscription-prices').val();
    var data = { "priceId": priceId };
    var priceControl = $('.plan-price');
    var stripeButton = $('.stripe-button');

    priceControl.empty();
    $.getJSON(url, data, function (planPrice) {
        if (planPrice !== null) {
            var currency = planPrice / 100;
            priceControl.append($('<span />', {
                text: currency + ':-'
            }));

            stripeButton.attr('data-amount', planPrice);
        }
    });
}

function onProductChange() {
    $('.subscription-products').on('change', function () {
        loadPlans();
    });
}

function onPlanChange() {
    $('.subscription-prices').on('change', function () {
        loadPlanPrice();
    });
}

$('#cancelSub').on('click', function () {
    var subscriptionId = $(this).attr('data-id');
    $.ajax({
        type: 'GET',
        url: '/User/Subscription?handler=CancelSubscription',
        dataType: 'json',
        data: {
            subscriptionId: subscriptionId
        },
        success: function (count) {
            $(window.location.reload());
        }
    })
})