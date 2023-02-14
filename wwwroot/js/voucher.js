$('.apply-discount-button').click(function () {
    var discountcode = document.getElementsByClassName('discount-input')[0].value
    var discount_card = document.getElementById('discount-card')
    console.log(discountcode)
    $.ajax({
        type: "POST",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val())
        },
        dataType: "json", 
        data: { code: discountcode },
        url: "/user/MealKitSubscription/Order?handler=CheckDiscount",
        //data: JSON.stringify({ code: discountcode, status: "check" }),
        success: function (data) {
            console.log(data)
            if (data.status == "Valid") {
                new SnackBar({
                    message: "Discount has been applied",
                    status: "success",
                    fixed: true,
                })
                discount_card.style.display = "";
                $('.cart-discount').text("Discount of " + data.discountAmt + "% has been applied!!")
                $('#discount_code_entered').val(discountcode)
                console.log($('#discount_code_entered'))
                console.log(discountcode)
            }
            else if (data.status == "InvalidCode") {
                new SnackBar({
                    message: "Invalid Code",
                    status: "error",
                    fixed: true,
                })
                discount_card.style.cssText = "display:none !important;";
                $('.cart-discount').text(0 + "%")
                $('#discount_code_entered').val("Null")
            }
            else if (data.status == "Expired") {
                new SnackBar({
                    message: "This voucher has expired!",
                    status: "error",
                    fixed: true
                })
                discount_card.style.cssText = "display:none !important;";
                $('.cart-discount').text(0 + "%")
                $('#discount_code_entered').val("Null")
            }
            else if (data.status == "ran_out") {
                new SnackBar({
                    message: "Be faster next time! Voucher has been used up",
                    status: "error",
                    fixed: true
                })
                discount_card.style.cssText = "display:none !important;";
                $('.cart-discount').text(0 + "%")
                $('#discount_code_entered').val("Null")
            }
            else {
                new SnackBar({
                    message: "Please contact an admin to fix the issue!",
                    status: "error",
                    fixed: true
                })
                discount_card.style.cssText = "display:none !important;";
                $('.cart-discount').text(0 + "%")
                $('#discount_code_entered').val("Null")
            }
        }
    })
    console.log("hello1")
    //$.ajax({
    //    url: "/discount",
    //    method: 'POST',
    //    contentType: "application/json",
    //    data: JSON.stringify({ code: discountcode, status: "check" }),
    //    success: function (res) {
    //        if (res.status == "success") {
    //            new SnackBar({
    //                message: "Discount has been applied!",
    //                status: "success",
    //                fixed: true
    //            })
    //            discount_card.style.display = "";
    //            $('.cart-discount').text(res.discount_amount + "%")
    //            console.log(discountcode)
    //            $('#discount_code_entered').val(discountcode)
    //            localStorage.setItem("discount_amount", res.discount_amount)
    //            disc()
    //        } else if (res.status == "spools_shortage") {
    //            new SnackBar({
    //                message: "You do not have enough spools to use this voucher!",
    //                status: "error",
    //                fixed: true
    //            })
    //            discount_card.style.cssText = "display:none !important;";
    //            $('.cart-discount').text(0 + "%")
    //            $('#discount_code_entered').val("")
    //            localStorage.setItem("discount_amount", "")
    //            disc()
    //        } else if (res.status == "voucher_expired") {
    //            new SnackBar({
    //                message: "This voucher has expired!",
    //                status: "error",
    //                fixed: true
    //            })
    //            discount_card.style.cssText = "display:none !important;";
    //            $('.cart-discount').text(0 + "%")
    //            $('#discount_code_entered').val("")
    //            localStorage.setItem("discount_amount", "")
    //            disc()
    //        } else if (res.status == "voucher_ran_out") {
    //            new SnackBar({
    //                message: "Be faster next time! Voucher has been used up",
    //                status: "error",
    //                fixed: true
    //            })
    //            discount_card.style.cssText = "display:none !important;";
    //            $('.cart-discount').text(0 + "%")
    //            $('#discount_code_entered').val("")
    //            localStorage.setItem("discount_amount", "")
    //            disc()
    //        } else if (res.status == "no_such_voucher") {
    //            new SnackBar({
    //                message: "There is no such voucher!",
    //                status: "error",
    //                fixed: true
    //            })
    //            discount_card.style.cssText = "display:none !important;";
    //            $('.cart-discount').text(0 + "%")
    //            $('#discount_code_entered').val("")
    //            localStorage.setItem("discount_amount", "")
    //            disc()
    //        } else {
    //            new SnackBar({
    //                message: "Please contact an admin to fix the issue!",
    //                status: "error",
    //                fixed: true
    //            })
    //            discount_card.style.cssText = "display:none !important;";
    //            $('.cart-discount').text(0 + "%")
    //            $('#discount_code_entered').val("")
    //            localStorage.setItem("discount_amount", "")
    //            disc()
    //        }
    //        setTimeout(() => checkoutsave(), 50);
    //    }
    //})
})