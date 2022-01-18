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
                    url: "/Admin/Referanslar/ReferanslariGetir",
                    type: "POST",
                    data: {
                        columnsDef: ["Id", "ReferansAdi", "SeoUrl", "ArkaPlanResmi", "Resim", "KisaAciklama", "İşlem"]
                    }
                },
                columns: [{
                    data: "Id"
                }, {
                    data: "ReferansAdi"
                }, {
                    data: "Resim"
                }, {
                    data: "Id"
                }],
                initComplete: function (x) {
                    var a = $('<tr class="filter"></tr>').appendTo($(t.table().header()));
                    this.api().columns().every(function () {
                        var e;
                        switch (this.title()) {
                            case "Referans Id":
                            case "Referans Adı":
                            case "Seo Url":
                                e = $('<input type="text" class="form-control form-control-sm form-filter m-input" data-col-index="' + this.index() + '"/>');
                                break;
                            case "İşlem":
                                var i = $('<button class="btn btn-brand m-btn btn-sm m-btn--icon">\n\t\t\t\t\t\t\t  <span>\n\t\t\t\t\t\t\t    <i class="la la-search"></i>\n\t\t\t\t\t\t\t    <span>Ara</span>\n\t\t\t\t\t\t\t  </span>\n\t\t\t\t\t\t\t</button>'),
                                    s = $('<button class="btn btn-secondary m-btn btn-sm m-btn--icon">\n\t\t\t\t\t\t\t  <span>\n\t\t\t\t\t\t\t    <i class="la la-close"></i>\n\t\t\t\t\t\t\t    <span>Reset</span>\n\t\t\t\t\t\t\t  </span>\n\t\t\t\t\t\t\t</button>');
                                $("<th>").append(i).append(s).appendTo(a), $(i).on("click", function (e) {
                                    e.preventDefault();
                                    var n = {};
                                    $(a).find(".m-input").each(function () {
                                        var t = $(this).data("col-index");
                                        n[t] ? n[t] += "|" + $(this).val() : n[t] = $(this).val()
                                    }), $.each(n, function (a, e) {
                                        t.column(a).search(e || "", !1, !1)
                                    }), t.table().draw()
                                }), $(s).on("click", function (e) {
                                    e.preventDefault(), $(a).find(".m-input").each(function (a) {
                                        $(this).val(""), t.column($(this).data("col-index")).search("", !1, !1);
                                    }), t.table().draw();
                                });
                        }
                        $(e).appendTo($("<th>").appendTo(a));
                    });

                    $('#m_table_1 tbody').on('click', '.referans-sil', function () {
                        var clickItem = $(this);
                        var referansId = $(this).data("referansid");
                        if (referansId === null || referansId === undefined || referansId === "") {
                            alert("Referans bulunamadı, Lütfen sayfayı yenileyiniz");
                            return;
                        }

                        Swal({
                            title: 'Bu referansı silmek istediğinize emin misiniz?',
                            text: "Eğer bu referansı silerseniz, bu referans hakkındaki bilgiler silinecektir!",
                            type: 'warning',
                            showCancelButton: true,
                            confirmButtonColor: '#009de1',
                            cancelButtonColor: '#ed8086',
                            confirmButtonText: 'Evet, Sil !',
                            cancelButtonText: 'Vazgeç'
                        }).then((result) => {
                            if (result.value) {
                                $.ajax({
                                    url: "/Admin/Referanslar/ReferansSil?referansId=" + referansId,
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
                        return '\n   <a href="/Admin/Referanslar/ReferansDuzenle?referansid=' + t + '" class="m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" title="Düzenle">\n<i class="la la-edit"></i>\n</a> \n<a href="javascript:void(0)" class="m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill referans-sil" data-referansid=' + t + ' title="Sil">\n<i class="la la-close"></i>\n</a> \n';
                    }
                },
                {
                    targets: 0,
                    width: 10
                }, {
                    targets: 2,
                    width: 60,
                    render: function (t, a, e, n) {
                        return '<img src="' + t + '" style="width:50px;height:50px;display: flex;margin: 0 auto;"/>';
                    } 
                }, {
                    targets: 3,
                    width: 60,
                    render: function (t, a, e, n) {
                        return '<img src="' + t + '" style="width:50px;height:50px;display: flex;margin: 0 auto;"/>';
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