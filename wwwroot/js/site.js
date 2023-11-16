// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function FillStates(lstCountryCtrl, lstStateId) {

    var lstStates = $("#" + lstStateId);
    lstStates.empty();



    var selectedCountry = lstCountryCtrl.options[lstCountryCtrl.selectedIndex].value;

    if (selectedCountry != null && selectedCountry != '') {
        $.getJSON("/home/GetStatesByCountry", { countryId: selectedCountry }, function (states) {
            if (states != null && !jQuery.isEmptyObject(states)) {
                $.each(states, function (index, state) {
                    lstStates.append($('<option/>',
                        {
                            value: state.value,
                            text: state.text
                        }));
                });
            };
        });
    }

    return;
}
