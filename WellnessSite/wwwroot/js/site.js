// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function changeColour(property, colour) {
    console.log(property);
    console.log(colour);
    document.documentElement.style.setProperty(property, colour);
}

function changeColours(prefs) {
    changeColour('--text', prefs[0]);
    changeColour('--highColour', prefs[1]);
    changeColour('--backColour', prefs[2]);
    changeColour('--headColour', prefs[3]);
    changeColour('--headText', prefs[4]);
    changeColour('--footColour', prefs[5]);
    changeColour('--footText', prefs[6]);
    changeColour('--linkColour', prefs[7]);
    changeColour('--hex1', prefs[8]);
    changeColour('--hex2', prefs[9]);
    changeColour('--hexColour', prefs[10]);
    changeColour('--hexHover', prefs[11]);
    changeColour('--textSize', prefs[12] + 'px');
}

function display(property) {
    if (document.getElementById(property).style.display == "block") {
        document.getElementById(property).style.display = "none";
    }
    else {
        document.getElementById(property).style.display = "block";
    }
    
}

function changeHexMediaQueries(size) {
    sheetnum = 3;
    for (i = 0; i < document.styleSheets.length; i++) {
        if (document.styleSheets[i] != null && document.styleSheets[i].href.includes('hex')) {
            sheetnum = i;
            break;
        }
    }

    document.styleSheets[sheetnum].cssRules[0].media.mediaText = '(max-width: ' + (size * 2) + 'px)';
    document.styleSheets[sheetnum].cssRules[1].media.mediaText = '(min-width: ' + (size * 2) + 'px) and (max-width: ' + (size * 3) + 'px)';
    document.styleSheets[sheetnum].cssRules[2].media.mediaText = '(min-width: ' + (size * 3) + 'px) and (max-width: ' + (size * 4) + 'px)';
    document.styleSheets[sheetnum].cssRules[3].media.mediaText = '(min-width: ' + (size * 4) + 'px) and (max-width: ' + (size * 5) + 'px)';
    document.styleSheets[sheetnum].cssRules[4].media.mediaText = '(min-width: ' + (size * 5) + 'px)';
}