var DatatablesSearchOptionsColumnSearch = function () {
    $.fn.dataTable.Api.register("column().title()", function () {
        return $(this.header()).text().trim();
    });
    return {
        init: function () {
            var t;
            t = $("#m_table_1").DataTable({
                responsive: !0,
                dom: "<'row'<'col-sm-12'tr>>\n\t\t\t<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>",
                lengthMenu: [5, 10, 25, 50],
                pageLength: 10,
                language: {
                    lengthMenu: "Display _MENU_"
                },
                searchDelay: 500,
                processing: !0,
                serverSide: !0,
                ajax: {
                    url: "/Admin/Urunler/UrunleriGetir",
                    type: "POST",
                    data: {
                        columnsDef: ["Id", "UrunResmi", "UrunAdi", "Keywords", "KisaAciklama", "Durum", "OlusturmaTarihi", "KategoriAdi", "EditUrun", "Dil", "İşlem"]
                    }
                },
                columns: [{
                    data: "Id"
                }, {
                    data: "UrunResmi"
                }, {
                    data: "UrunAdi"
                }, {
                    data: "Keywords"
                },
                {
                    data: "KisaAciklama"
                }, {
                    data: "KategoriAdi"
                }, {
                    data: "Dil"
                }, {
                    data: "Durum"
                }, {
                    data: "OlusturmaTarihi"
                }, {
                    data: "EditUrun"
                }],
                initComplete: function (x) {
                    var a = $('<tr class="filter"></tr>').appendTo($(t.table().header()));
                    this.api().columns().every(function () {
                        var e;
                        switch (this.title()) {
                            case "Ürün Id":
                            case "Ürün Adı":
                            case "Keywords":
                            case "Ürün Kategorisi":
                                e = $('<input type="text" class="form-control form-control-sm form-filter m-input" data-col-index="' + this.index() + '"/>');
                                break;
                            case "Durum":
                                var n = [
                                    {
                                        title: "Aktif",
                                        class: "m-badge--brand",
                                        value: 1
                                    },
                                    {
                                        title: "Pasif",
                                        class: " m-badge--metal",
                                        value: 2
                                    }
                                ];
                                e = $('<select class="form-control form-control-sm form-filter m-input" title="Select" data-col-index="' + this.index() + '">\n\t\t\t\t\t\t\t\t\t\t<option value="">Seçiniz</option></select>');

                                for (var k = 0; k < n.length; k++) {
                                    $(e).append('<option value="' + n[k].value + '">' + n[k].title + "</option>");
                                }
                                break;
                            case "İşlem":
                                var i = $('<button class="btn btn-brand m-btn btn-sm m-btn--icon">\n\t\t\t\t\t\t\t  <span>\n\t\t\t\t\t\t\t    <i class="la la-search"></i>\n\t\t\t\t\t\t\t    <span>Ara</span>\n\t\t\t\t\t\t\t  </span>\n\t\t\t\t\t\t\t</button>'),
                                    s = $('<button class="btn btn-secondary m-btn btn-sm m-btn--icon">\n\t\t\t\t\t\t\t  <span>\n\t\t\t\t\t\t\t    <i class="la la-close"></i>\n\t\t\t\t\t\t\t    <span>Reset</span>\n\t\t\t\t\t\t\t  </span>\n\t\t\t\t\t\t\t</button>');
                                $("<th>").append(i).append(s).appendTo(a), $(i).on("click", function (e) {
                                    e.preventDefault();
                                    var n = {};
                                    $(a).find(".m-input").each(function () {
                                        var t = $(this).data("col-index");
                                        n[t] ? n[t] += "|" + $(this).val() : n[t] = $(this).val();
                                    }), $.each(n, function (a, e) {
                                        t.column(a).search(e || "", !1, !1);
                                    }), t.table().draw();
                                }), $(s).on("click", function (e) {
                                    e.preventDefault(), $(a).find(".m-input").each(function (a) {
                                        $(this).val(""), t.column($(this).data("col-index")).search("", !1, !1);
                                    }), t.table().draw();
                                });
                        }
                        $(e).appendTo($("<th>").appendTo(a));
                    });

                    $('#m_table_1 tbody').on('click', '.urun-sil', function () {
                        var clickItem = $(this);
                        var urunId = $(this).data("urunid");
                        if (urunId === null || urunId === undefined || urunId === "") {
                            alert("Ürün bulunamadı, Lütfen sayfayı yenileyiniz");
                            return;
                        }

                        Swal({
                            title: 'Bu Ürünü silmek istediğinize emin misiniz?',
                            text: "Eğer bu ürünü silerseniz, bu üründeki tüm veriler silinecektir!",
                            type: 'warning',
                            showCancelButton: true,
                            confirmButtonColor: '#009de1',
                            cancelButtonColor: '#ed8086',
                            confirmButtonText: 'Evet, Sil !',
                            cancelButtonText: 'Vazgeç'
                        }).then((result) => {
                            if (result.value) {
                                $.ajax({
                                    url: "/Admin/Urunler/UrunSil?urunId=" + urunId,
                                    type: "POST",
                                    contentType: "application/json",
                                    success: function (data) {

                                        if (data.BasariliMi) {
                                            t.row(clickItem.parents('tr')).remove().draw(false);
                                            Swal(
                                                'Silme işlemi başarılı!',
                                                data.Mesaj,
                                                'success'
                                            );
                                        }
                                        else {
                                            Swal(
                                                'Silme işlemi başarısız!',
                                                data.Mesaj,
                                                'error'
                                            );
                                        }
                                    },
                                    beforeSend: function () {

                                    },
                                    error: function (data) {
                                        Swal(
                                            'Silme işlemi başarısız!',
                                            'Beklenmedik bir hata meydana geldi',
                                            'error'
                                        );
                                    }
                                });
                            }
                        });
                    });
                },
                columnDefs: [{
                    targets: -1,
                    title: "İşlem",
                    width: 50,
                    orderable: !1,
                    render: function (t, a, e, n) {
                        return '\n<span class="dropdown">\n <a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">\n                              <i class="la la-ellipsis-h"></i>\n                              </a>\n                            <div class="dropdown-menu dropdown-menu-right">\n                                <a class="dropdown-item" target="_blank" href="/Admin/Gezi/GeziDuzenle?urunId=' + t.Id + '"><i class="la la-edit"></i>Gezi Detay</a>\n                                             <a class="dropdown-item urun-sil" href="javascript:void(0);" data-urunid=' + t.Id + '><i class="la la-close"></i>Gezi Sil</a>\n                            </div>\n                        </span>\n                        <a href="/Admin/Gezi/GeziDuzenle?urunId=' + t.Id + '" class="m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" title="Düzenle">\n                          <i class="la la-edit"></i>\n                        </a>';
                    }
                },
                {
                    targets: 0,
                    width: 10
                }, {
                    targets: 1,
                    width: 60,
                    render: function (t, a, e, n) {
                        return '<img src="' + t + '" style="width:50px;height:50px;display: flex;margin: 0 auto;"/>';
                    }
                },
                {
                    targets: 6,
                    render: function (t, a, e, n) {
                        var i = {
                            1: {
                                title: "Aktif",
                                class: "m-badge--brand"
                            },
                            2: {
                                title: "Pasif",
                                class: " m-badge--metal"
                            },
                        };
                        return void 0 === i[t] ? t : '<span class="m-badge ' + i[t].class + ' m-badge--wide">' + i[t].title + "</span>";
                    }
                }, {
                    targets: 5,
                    render: function (t, a, e, n) {
                        var i = {
                            1: {
                                title: "Türkçe",
                                class: "badge badge-kredikarti"

                            },
                            2: {
                                title: "İngilizce",
                                class: "badge badge-havale"

                            }
                        };
                        return void 0 === i[t] ? t : '<span class="m-badge ' + i[t].class + ' m-badge--wide">' + i[t].title + "</span>";
                    }
                }
                ]
            });
        }
    };
}();
jQuery(document).ready(function () {
    DatatablesSearchOptionsColumnSearch.init();
});