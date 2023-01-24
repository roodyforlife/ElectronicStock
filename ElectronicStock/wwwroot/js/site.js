function createPDF(title) {
    var sTable = $('#print').html()
    var style = "<style>";
    style = style + "table {width: 100%;font: 17px Calibri;}";
    style = style + "table, th, td {border: solid 1px #DDD; border-collapse: collapse;";
    style = style + "padding: 2px 3px;text-align: center;}";
    style += Styles();
    style = style + ".createPdfBlock {display: none;}";
    style = style + "</style>";

    var win = window.open('', '', 'height=700,width=700');
    win.document.write('<html><head>');
    win.document.write(`<title>${title}</title>`);
    win.document.write(style);
    win.document.write('</head>');
    win.document.write('<body>');
    win.document.write(`<h1 style='text-align: center;'>${title}</h1>`);
    win.document.write(sTable);
    win.document.write(`<h4 style='text-align: left;'>Signature: <span style="font-style: italic; color: blue;">Electronik Stock</span></h4>`);
    win.document.write('</body></html>');
    win.document.close();
    win.print();
}

function Styles() {
    var CSSFile;
    var rawFile = new XMLHttpRequest();
    rawFile.open("GET", `/lib/bootstrap/dist/css/bootstrap.css`, false);
    rawFile.onreadystatechange = function () {
        if (rawFile.readyState === 4) {
            if (rawFile.status === 200) {
                CSSFile = rawFile.responseText;
            }
        }
    }

    rawFile.send(null);
    return CSSFile;
}