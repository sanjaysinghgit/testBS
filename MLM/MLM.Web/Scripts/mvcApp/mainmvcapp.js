$(function () {

    var isDirty = false;
    var URL_CONFIG;

    URL_CONFIG = {
            UI_LANGUAGE: "en",
            SERVICE_BASE: function () {
                var base = window.location.protocol + "//" + window.location.host;
                return base;
            }
        };

    $('#SponsorCode').keyup(function () {
        if (!isDirty) {
            isDirty = true;
            console.log("inkeyup");
            validateSponsorCode();
        }
        
    });
    $('#SponsorCode').change(function () {
        
        if (!isDirty) {
            isDirty = true;
            console.log("change");
            validateSponsorCode();
        }

    });

    function validateSponsorCode()
    {
        var selectedPosition = $("input[type='radio'][name='Position']:checked").val();

        var sponsorCode = $("#SponsorCode").val();
        if (sponsorCode.length < 10) {
            isDirty = false;
            return;

        }

        getAgentApplicableSponsorcode(sponsorCode, selectedPosition);
    }

    function getAgentApplicableSponsorcode(sponsorCode, selectedPosition)
    {
        $('#AgentApplicableSponsorCode').val("");
        jQuery.support.cors = true;
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: URL_CONFIG.SERVICE_BASE() + '/api/agent/getagentapplicablesponsorcode',
            data: { "providedSponsorCode": sponsorCode, "agentPosition": selectedPosition },
            dataType: "json",
            beforeSend: function () {
                //alert(sponsorCode + " " + selectedPosition);
            },
            success: function (data) {
                // set model prop AgentApplicableSponsorCode
                $('#AgentApplicableSponsorCode').val(data);
                console.log($('#AgentApplicableSponsorCode').val());
                isDirty = false;
                if ($('#AgentApplicableSponsorCode').val() == "0") {
                    alert("Sponsor code: '" + $('#SponsorCode').val() + "' is not valid.");
                    $('#SponsorCode').val("");

                    return;
                }

                // And display alert if AgentApplicableSponsorCode != sponsorcode
                if ($('#SponsorCode').val() != $('#AgentApplicableSponsorCode').val()) {
                    isDirty = false;
                    alert("This joining falls under spill over");
                    return;
                }

            },
            error: function (err) {
                //display msg in msgAgentApplicableSponsorCode
                isDirty = false;
                console.log(err);
            }
        });


    }
});