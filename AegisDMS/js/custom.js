
function openpopup(poplocation) {
    var popposition = 'left = 250, top=40, width=1000,align=center, height=580,menubar=no, scrollbars=yes, resizable=yes';

    var NewWindow = window.open(poplocation, '', popposition);
    if (NewWindow.focus != null) {
        NewWindow.focus();
    }
}
