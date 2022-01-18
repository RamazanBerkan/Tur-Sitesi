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
                    url: "/admin/Bayiler/BayileriGetir",
                    type: "POST",
                    data: {
                        columnsDef: ["ProfilResmi", "Id", "Ad", "Soyad", "TCKN", "OlusturmaTarihi", "Durum", "UserName", "Actions"]
                    }
                },
                columns: [{
                    data: "ProfilResmi"
                }, {
                    data: "Id"
                }, {
                    data: "Ad"
                }, {
                    data: "Soyad"
                }, {
                    data: "TCKN"
                }, {
                    data: "OlusturmaTarihi"
                }, {
                    data: "Durum"
                }, {
                    data: "UserName"
                }, {
                    data: "Actions"
                }],
                initComplete: function () {
                    var a = $('<tr class="filter"></tr>').appendTo($(t.table().header()));
                    this.api().columns().every(function () {
                        var e;
                        switch (this.title()) {
                            case "Kullanici Id":
                            case "Profil Resmi":
                            case "Ad":
                            case "Soy Ad":
                                e = $('<input type="text" class="form-control form-control-sm form-filter m-input" data-col-index="' + this.index() + '"/>');
                                break;
                            case "TCKN":
                                e = $('<select class="form-control form-control-sm form-filter m-input" title="Select" data-col-index="' + this.index() + '">\n\t\t\t\t\t\t\t\t\t\t<option value="">Select</option></select>'), this.data().unique().sort().each(function (t, a) {
                                    $(e).append('<option value="' + t + '">' + t + "</option>")
                                });
                                break;
                            case "Durum":
                                var n = {
                                    1: {
                                        title: "Aktif",
                                        class: "m-badge--brand"
                                    },
                                    2: {
                                        title: "Pasif",
                                        class: " m-badge--metal"
                                    }
                                };
                                e = $('<select class="form-control form-control-sm form-filter m-input" title="Select" data-col-index="' + this.index() + '">\n\t\t\t\t\t\t\t\t\t\t<option value="">Seçiniz</option></select>'), this.data().unique().sort().each(function (t, a) {
                                    $(e).append('<option value="' + t + '">' + n[t].title + "</option>")
                                });
                                break;
                            case "Kullanici Adi":
                                e = $('<input type="text" class="form-control form-control-sm form-filter m-input" data-col-index="' + this.index() + '"/>');
                                break;
                            case "Olusturulma Tarihi":
                                e = $('\n\t\t\t\t\t\t\t<div class="input-group date">\n\t\t\t\t\t\t\t\t<input type="text" class="form-control form-control-sm m-input" readonly placeholder="From" id="m_datepicker_1"\n\t\t\t\t\t\t\t\t data-col-index="' + this.index() + '"/>\n\t\t\t\t\t\t\t\t<div class="input-group-append">\n\t\t\t\t\t\t\t\t\t<span class="input-group-text"><i class="la la-calendar-o glyphicon-th"></i></span>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t<div class="input-group date">\n\t\t\t\t\t\t\t\t<input type="text" class="form-control form-control-sm m-input" readonly placeholder="To" id="m_datepicker_2"\n\t\t\t\t\t\t\t\t data-col-index="' + this.index() + '"/>\n\t\t\t\t\t\t\t\t<div class="input-group-append">\n\t\t\t\t\t\t\t\t\t<span class="input-group-text"><i class="la la-calendar-o glyphicon-th"></i></span>\n\t\t\t\t\t\t\t\t</div>\n\t\t\t\t\t\t\t</div>');
                                break;
                            case "Actions":
                                var i = $('<button class="btn btn-brand m-btn btn-sm m-btn--icon">\n\t\t\t\t\t\t\t  <span>\n\t\t\t\t\t\t\t    <i class="la la-search"></i>\n\t\t\t\t\t\t\t    <span>Search</span>\n\t\t\t\t\t\t\t  </span>\n\t\t\t\t\t\t\t</button>'),
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
                                        $(this).val(""), t.column($(this).data("col-index")).search("", !1, !1)
                                    }), t.table().draw()
                                })
                        }
                        $(e).appendTo($("<th>").appendTo(a))
                    }), $("#m_datepicker_1,#m_datepicker_2").datepicker()
                },
                columnDefs: [{
                    targets: -1,
                    title: "Actions",
                    orderable: !1,
                    render: function (t, a, e, n) {
                        return '\n<span class="dropdown">\n <a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">\n                              <i class="la la-ellipsis-h"></i>\n                            </a>\n                            <div class="dropdown-menu dropdown-menu-right">\n                                <a class="dropdown-item" href="#"><i class="la la-edit"></i> Edit Details</a>\n                                <a class="dropdown-item" href="#"><i class="la la-leaf"></i> Update Status</a>\n                                <a class="dropdown-item" href="#"><i class="la la-print"></i> Generate Report</a>\n                            </div>\n                        </span>\n                        <a href="#" class="m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" title="View">\n                          <i class="la la-edit"></i>\n                        </a>'
                    }
                },
                {
                    targets: 5,
                    width: "150px"
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
                            3: {
                                title: "Canceled",
                                class: " m-badge--primary"
                            },
                            4: {
                                title: "Success",
                                class: " m-badge--success"
                            },
                            5: {
                                title: "Info",
                                class: " m-badge--info"
                            },
                            6: {
                                title: "Danger",
                                class: " m-badge--danger"
                            },
                            7: {
                                title: "Warning",
                                class: " m-badge--warning"
                            }
                        };
                        return void 0 === i[t] ? t : '<span class="m-badge ' + i[t].class + ' m-badge--wide">' + i[t].title + "</span>"
                    }
                },
                {
                    targets: 7,
                    render: function (t, a, e, n) {
                        var i = {
                            1: {
                                title: "Online",
                                state: "danger"
                            },
                            2: {
                                title: "Retail",
                                state: "primary"
                            },
                            3: {
                                title: "Direct",
                                state: "accent"
                            }
                        };
                        return void 0 === i[t] ? t : '<span class="m-badge m-badge--' + i[t].state + ' m-badge--dot"></span>&nbsp;<span class="m--font-bold m--font-' + i[t].state + '">' + i[t].title + "</span>"
                    }
                },
                {
                    targets: 0,
                    width: "60px",
                    render: function (t, a, e, n) {
                        return '<img src="' + t + '" style="width:50px;height:50px;display: flex;margin: 0 auto;"/>';
                    }
                 },
                 {
                        targets: 1,
                        width: "150px",
                 }
                ]
            })
        }
    }
}();
jQuery(document).ready(function () {
    DatatablesSearchOptionsColumnSearch.init();
});