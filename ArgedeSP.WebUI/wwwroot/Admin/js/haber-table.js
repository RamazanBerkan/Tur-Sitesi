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
                    url: "/Admin/Haberler/HaberleriGetir",
                    type: "POST",
                    data: {
                        columnsDef: ["Id", "Ad", "Resim",  "KisaAciklama", "Dil", "İşlem"]
                    }
                },
                //select: {
                //    style: 'multi'
                //},
                columns: [{
                    data: "Id"
                }, {
                    data: "Ad"
                }, {
                    data: "Resim"
                }, {
                    data: "Dil"
                }, {
                    data: "Id"
                }],
                initComplete: function (x) {
                    var a = $('<tr class="filter"></tr>').appendTo($(t.table().header()));
                    this.api().columns().every(function () {
                        var e;
                        switch (this.title()) {
                            case "Haber Id":
                            case "Haber Adı":
                                e = $('<input type="text" class="form-control form-control-sm form-filter m-input" data-col-index="' + this.index() + '"/>');
                                break;
                            case "Dil":
                                var n = [
                                    {
                                        title: "Türkçe",
                                        class: "badge badge-kredikarti",
                                        value: 1
                                    },
                                    {
                                        title: "İngilizce",
                                        class: "badge badge-havale",
                                        value: 2
                                    }
                                ];

                                e = $('<select class="form-control form-control-sm form-filter m-input" title="Select" data-col-index="' + this.index() + '">\n\t\t\t\t\t\t\t\t\t\t<option value="">Dil Seçiniz</option></select>');

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

                    $('#m_table_1 tbody').on('click', '.haber-sil', function () {
                        var clickItem = $(this);
                        var haberId = $(this).data("haberid");
                        if (haberId === null || haberId === undefined || haberId === "") {
                            alert("Haber bulunamadı, Lütfen sayfayı yenileyiniz");
                            return;
                        }

                        Swal({
                            title: 'Bu haberi silmek istediğinize emin misiniz?',
                            text: "Eğer bu haber silerseniz, bu haberde ki veriler silinecektir!",
                            type: 'warning',
                            showCancelButton: true,
                            confirmButtonColor: '#009de1',
                            cancelButtonColor: '#ed8086',
                            confirmButtonText: 'Evet, Sil !',
                            cancelButtonText: 'Vazgeç'
                        }).then((result) => {
                            if (result.value) {
                                $.ajax({
                                    url: "/Admin/Haberler/HaberSil?haberId=" + haberId,
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
                        return '\n   <a href="/Admin/Haberler/HaberDuzenle?haberId=' + t + '" class="m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" title="Düzenle">\n<i class="la la-edit"></i>\n</a> \n<a href="javascript:void(0)" class="m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill haber-sil" data-haberid=' + t + ' title="Sil">\n<i class="la la-close"></i>\n</a> \n';
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