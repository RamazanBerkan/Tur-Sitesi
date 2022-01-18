$(function () {

    (function ($) {
        $.fn.serializeFormJSON = function () {

            var o = {};
            var a = this.serializeArray();
            $.each(a, function () {
                if (o[this.name]) {
                    if (!o[this.name].push) {
                        o[this.name] = [o[this.name]];
                    }
                    o[this.name].push(this.value || '');
                } else {
                    o[this.name] = this.value || '';
                }
            });
            return o;
        };
    })(jQuery);
    $("body").on("click", "#uruntipi-ekle", function () {
        event.preventDefault();

        var myform = $("#uruntipi-ekle-form").serialize() + "&UrunTipiEkle_REQ.UrunId=" + $("#UrunEkle_REQ_Id").val();

        $.ajax({
            url: "/Admin/Urunler/UrunTipiEkle",
            type: "POST",
            data: myform,
            success: function (data) {
                $("#urun-tipleri").html(data);
            },
            beforeSend: function () {

            },
            error: function (data) {

            }
        });
    })
    $("body").on("click", ".uruntipi-sil", function () {

        var urunTipiId = $(this).data("value");

        $.ajax({
            url: "/Admin/Urunler/UrunTipiSil?urunTipiId=" + urunTipiId,
            type: "POST",
            success: function (data) {
                if (data.BasariliMi != undefined) {
                    alert("Ürün tipi bulunamadı")
                }
                else {
                    $("#urun-tipleri").html(data);

                }
            },
            beforeSend: function () {

            },
            error: function (data) {

            }
        });



    })
    $("body").on("click", ".uruntipi-guncelle-get", function () {

        var urunTipiId = $(this).data("value");
        $.ajax({
            url: "/Admin/Urunler/UrunTipiDuzenle?urunTipiId=" + urunTipiId,
            type: "GET",
            success: function (data) {
                if (data.BasariliMi != undefined) {
                    alert("Ürün tipi bulunamadı")
                }
                else {
                    $("#uruntipi-duzenle-modal-body").html(data);
                    $("#uruntipi-duzenle-modal").modal("show")
                }
            },
            beforeSend: function () {

            },
            error: function (data) {

            }
        });
    });
    $("body").on("click", "#uruntipi-guncelle-post", function () {

        var myform = $("#uruntipi-duzenle-form").serialize();

        $.ajax({
            url: "/Admin/Urunler/UrunTipiDuzenle",
            type: "POST",
            data: myform,
            success: function (data) {
                if (data.BasariliMi) {
                    $("#uruntipi-duzenle-modal").modal("hide");
                    $(".modal-backdrop").remove();
                    $("#urun-tipleri").html(data.Model.UrunTipleri);            
                }
                else {
                    $("#uruntipi-duzenle-modal-body").html(data.Model.UrunGuncelleModal);
                }

            },
            beforeSend: function () {

            },
            error: function (data) {

            }
        });
    });
});