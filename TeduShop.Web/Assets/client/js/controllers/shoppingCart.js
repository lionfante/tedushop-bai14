var cart = {
    init: function () {
        cart.loadData();
        cart.registerEvent();
    },
    registerEvent: function () {
        

        $('.btnDeleteItem').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));
            cart.deleteItem(productId);
        });

        $('.txtQuantity').off('keyup').on('keyup', function () {

            var quantity = parseInt($(this).val());
            var productId = parseInt($(this).data('id'));
            var price = parseFloat($(this).data('price'));

            if (isNaN(quantity) == false) {
                var amount = quantity * price;
                $('#amount_' + productId).text(numeral(amount).format('0,0'));
            } else {
                $('#amount_' + productId).text(0);
            }
            $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0'));
            cart.updateAll();
        });

        $('#btnContinue').off('click').on('click', function (e) {
            e.preventDefault();
            window.location.href = "/";
        });

        $('#btnDeleteAll').off('click').on('click', function (e) {
            e.preventDefault();
            cart.deleteAll();
        });

        $('#btnCheckout').off('click').on('click', function (e) {
            e.preventDefault();
            $('#divCheckOut').show();
        });

        $('#chkUserLoginInfo').off('click').on('click', function () {
            if($(this).prop('checked')){
                cart.getLoginUser();
            }else{
                $('#txtName').val("");
                $('#txtAddress').val("");
                $('#txtEmail').val("");
                $('#txtPhone').val("");
            }
            
        });

        $('#btnCreateOrder').off('click').on('click', function (e) {
            e.preventDefault();
            cart.createOrder();
        });
        
        
    },
    getLoginUser: function(){
        $.ajax({
            url: '/ShoppingCart/GetUser',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var user = response.data;
                    $('#txtName').val(user.FullName);
                    $('#txtAddress').val(user.Address);
                    $('#txtEmail').val(user.Email);
                    $('#txtPhone').val(user.PhoneNumber);
                }
            }
        });
    },

    createOrder: function () {
        var order = {
        CustomerName : $('#txtName').val(),
        CustomerEmail: $('#txtEmail').val(),
        CustomerAddress: $('#txtAddress').val(),
        CustomerMobile: $('#txtPhoneNumber').val(),
        CustomerMessage: $('#txtMessage').val(),
        PaymentMethod : "Thanh toán tiền mặt",
        Status : false
        
        };
        $.ajax({
            url: '/ShoppingCart/CreateOrder',
            type: 'POST',
            dataType: 'json',
            data: {
                orderViewModel: JSON.stringify(order)
            },
            success: function (response) {
                if (response.status) {
                    $('#divCheckout').hide();
                    cart.deleteAll();
                    setTimeout(function () {

                    }, 2000);
                    $('#cartContent').html("Cảm ơn bạn đã đặt hàng thành công, chúng tôi sẽ liên hệ sớm nhất.");
                }
            }
        });
    },

    getTotalOrder: function(){
        var listTextbox = $('.txtQuantity');
        var total = 0;
        $.each(listTextbox, function (i, item) {
            var number = parseInt($(item).val());
            var price = parseFloat($(item).data('price'));
            total += parseInt($(item).val()) * parseFloat($(item).data('price'));
        });
        return total;
    },
    deleteItem: function(productId){
        $.ajax({
            url: '/ShoppingCart/Delete',
            data: { productId: productId },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    alert('Xóa sản phẩm từ giỏ hàng thành công.');
                    cart.loadData();
                }
            }
        });
    },
    deleteAll: function () {
        $.ajax({
            url: '/ShoppingCart/DeleteAll',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                }
            }
        });
    },
    updateAll: function () {
        var cartList = [];
        $.each($('.txtQuantity'), function (i, item) {
            cartList.push({
                ProductId: $(item).data('id'),
                Quantity: $(item).val()
            });
        });

        $.ajax({
            url: '/ShoppingCart/Update',
            type: 'POST',
            data: {
                cartData: JSON.stringify(cartList)
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                    console.log('Update ok');
                }
            }
        });
    },
    loadData: function(){
        $.ajax({
            url: '/ShoppingCart/GetAll',
            type: 'GET',
            dataType: 'json',
            success: function(res){
                if (res.status) {

                    var template = $('#tplCart').html();
                    var html = '';
                    var data = res.data;

                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ProductId: item.ProductId,
                            ProductName: item.Product.Name,
                            Image: item.Product.Image,
                            Price: item.Product.Price,
                            PriceF: numeral(item.Product.Price).format('0,0'),
                            Quantity: item.Quantity,
                            Amount: numeral(item.Quantity * item.Product.Price).format('0,0')
                        });
                    });
                    $('#cartBody').html(html);
                    if (html == "") {
                        $('#cartContent').html("Không có sản phẩm nào trong giỏ hàng");
                    }
                    $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0'));
                    cart.registerEvent();
                }
            }
        });
    }
}

cart.init();