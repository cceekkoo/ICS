﻿$(document).ready(function () {
    $.getScript('//cdnjs.cloudflare.com/ajax/libs/summernote/0.8.4/summernote.min.js', function () {
        $('.summernote').summernote();
    });
    $("#all").hide();
});

$(function () {
    $("#table").DataTable({
        "ordering": false
    });
});

function scrollheight() {
    var height = $(document).height() - $('.user-panel').height() - $('.logo-lg').height() - $('.main-footer').height() - 21;
    $('.sidebar-menu').slimscroll({
        height: height
    });
    $('.modal-body').attr('style', 'max-height:' + (height - 50) + 'px;');
}

scrollheight();

$(window).resize(function () {
    scrollheight();
});

function nottranslated(index) {
    var valueid = $('#table tbody tr:nth-child(' + index + ') td:nth-last-child(2)').html();
    var ID = [];
    while (true) {
        if ($('#table tbody tr:nth-child(' + index + ') td:nth-last-child(2)').html() === valueid) {
            ID.push($('#table tbody tr:nth-child(' + index + ') td:nth-last-child(1)').html());
        }
        else break;
        index++;
    }
    $.post("/NotTranslated/Get", { ID }, function (data) {
        $("#Language_ID").empty();
        $.each(data, function (index, item) {
            $('#Language_ID').append($("<option></option>")
                .attr("value", item.ID)
                .html(item.Language_Short));
        });
    });
}

$(document).on("click", ".btn-edit", function () {
    var index = $(this).parent().parent().index() + 1;
    for (i = 1; i < $('#EditModal .form-group > div').length + 1; i++) {
        var value = $('#table tbody tr:nth-child(' + index + ') td:nth-child(' + i + ')').html();
        $('#EditModal .form-group > div:nth-child(' + i + ') .note-editable').html(value);
        $('#EditModal .form-group > div:nth-child(' + i + ')').children('textarea').html(value);
        $('#EditModal .form-group > div:nth-child(' + i + ')').children('input').val(value);
        $('#EditModal .form-group > div:nth-child(' + i + ') select').children().removeAttr("selected");
        $('#EditModal .form-group > div:nth-child(' + i + ') select').children().each(function () {
            if($(this).text() == value)
            {
                $(this).parent().val($(this).val());;
            }
        });
    }
    $('.form-group > div> label > span').html(null);
})

$(document).on("click", ".btn-add", function () {
    var index = $(this).parent().parent().index() + 1;
    $('#AddModal .form-group div input').children('textarea').html(null);
    $('#AddModal .form-group div').children('input').val(null);
    $('#AddModal .form-group div .note-editable').html(null);
    $('.text-danger').html(null);
})

$(document).on("click", ".btn-translate", function () {
    var index = $(this).parent().parent().index() + 1;
    $('#TranslateModal .form-group div input').children('textarea').html(null);
    $('#TranslateModal .form-group div').children('input').val(null);
    $('#TranslateModal .form-group div .note-editable').html(null);
    var id = $('#table tbody tr:nth-child(' + index + ') td:first-child').html();
    $('#TranslateModal .form-group .hidden:first-child').children('input').val(id);
    $('.text-danger').html(null);
    nottranslated(index);
})

$(document).on("click", ".btn-view", function () {
    var index = $(this).parent().parent().index() + 1;
    for (i = 1; i < $('#ViewModal .table-modal tr').length + 1; i++) {
        var value = $('#table tbody tr:nth-child(' + index + ') td:nth-child(' + (i + 1) + ')').html();
        $('#ViewModal .table-modal tr:nth-child(' + i + ') td:nth-child(2)').html(value);
    }
})

var deleteurl = $('#DeleteModal form').attr('action');
$(document).on("click", ".btn-delete", function () {
    $('#DeleteModal form').attr('action', null);
    var index = $(this).parent().parent().index() + 1;
    for (i = 1; i < $('#DeleteModal .table-modal tr').length + 1; i++) {
        var value = $('#table tbody tr:nth-child(' + index + ') td:nth-child(' + (i + 1) + ')').html();
        $('#DeleteModal .table-modal tr:nth-child(' + i + ') td:nth-child(2)').html(value);
    }
    var id = $('#table tbody tr:nth-child(' + index + ') td:nth-child(1)').text();
    $('#DeleteModal form').attr('action', '/' + deleteurl.split('/')[1] + '/' + deleteurl.split('/')[2] + '/' + id);
    $('.text-danger').html(null);
})

var imageurl = $('#ImageModal form').attr('action');
$(document).on("click", ".btn-upload", function () {
    $('#ImageModal form').attr('action', null);
    var index = $(this).parent().parent().index() + 1;
    var id = $('#table tbody tr:nth-child(' + index + ') td:nth-child(1)').text();
    $('#ImageModal form').attr('action', '/' + imageurl.split('/')[1] + '/' + imageurl.split('/')[2] + '/' + id);
    $('.text-danger').html(null);
})

$(document).on("submit", "form", function () {
    $("#all").show();
})