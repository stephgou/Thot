$(document).ready(function (e) {

    $('#btnSearch').on('click', function (e) {
        var searchCriteria = $('#tbSearch').val();
        searchCriteria = $.trim(searchCriteria);
        if (searchCriteria === '') {
            alert('Saisir des critères de recherche.');
        } else {
            disableForm();
            search(searchCriteria);
            enableForm();
        }
        e.stopPropagation();
        return false;
    })

});

function disableForm() {
    $('#tbSearch').attr('disabled', 'disabled');
    $('#btnSearch').button('loading');
}

function enableForm() {
    $('#tbSearch').removeAttr('disabled');
    $('#btnSearch').button('reset');
}

function search(searchCriteria) {
    var searchResultsTemplate = _.template($('#search-results-template').html());

    var url = '/search?search=' + encodeURIComponent(searchCriteria) + '&_=' + Math.random();
    var ajax = $.ajax({
        url: url,
        type: 'GET',
    });

    ajax.done(function (data, status, xhr) {
        var tbody = $('#tableTbody');
        tbody.empty();
        var blobs = JSON.parse(data);
        var rowsHtml = searchResultsTemplate({ 'blobs': blobs });
        tbody.append(rowsHtml);
    });

    ajax.fail(function (xhr, desc, err) {
        alert('Une erreur s\'est produite. Veuillez recommencer."');
    });
}
var KB = 1024;
var MB = KB * KB;
var GB = MB * KB;

function sizeAsString(size) {
    if (parseFloat(size) === size && size >= 0) {
        if (size < KB) {
            return size + " bytes";
        }
        else if (size < MB) {
            return parseFloat(Math.floor(size * 100 / KB) / 100).toFixed(2) + " KB";
        }
        else if (size < GB) {
            return parseFloat(Math.floor(size * 100 / MB) / 100).toFixed(2) + " MB";
        }
        else {
            return parseFloat(Math.floor(size * 100 / GB) / 100).toFixed(2) + " GB";
        }
    }
    return size;
}