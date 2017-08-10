var common = {
    init: function () {
        common.registerEvents();
    },
    registerEvents: function () {
        $("#txtKeyword").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Product/GetListProductByName",
                    dataType: "json",
                    data: {
                        keyword: request.term
                    },
                    success: function (data) {
                        response(data.data);
                    }
                });
            },
            focus: function (event, ui) {
                $("#txtKeyword").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $("#txtKeyword").val(ui.item.label);

                return false;
            }
        })
        .autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
              .append("<div>" + item.label + "</div>")
              .appendTo(ul);
        };

        $('#btnLogout').off('click').on('click', function (e) {
            e.preventDefault();
            $('#frmLogout').submit();
        });

        $('.btnAddToCart').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));
            $.ajax({
                url: '/ShoppingCart/Add',
                data: {
                    productId: productId
                },
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.status) {
                        alert('Thêm sản phẩm vào giỏ hàng thành công.');
                    } else {
                        alert(response.message);
                    }
                }
            });
        });
    }
}

common.init();