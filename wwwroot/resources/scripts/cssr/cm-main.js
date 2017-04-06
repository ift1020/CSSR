var cssr = (function () {
    var summary = {
        programID: 0,
        subcampaignID: 0,
        status: false,
        title: "",
        cadence: "",
        formats: "",
        checks: []
    }
    const defaultUrls = {
        target:   '/cm/programs/cssr/targettingDefault.aspx',
        offer:    '/cm/programs/cssr/offersDefault.aspx',
        creative: '/cm/programs/cssr/creativeDefault.aspx',
        service:  '/cm/programs/cssr/servicesDefault.aspx'
    }
    const cssrDoc_en = {
        header: 'Customer Sales & Service Retention Program',
        body: 'CSSR is a dynamic lifecycle customer retention communication program, and also one of the core elements of the GM Retail Excellence Program.<br/>' + 
            'To be compliant for REP and have a Green status for CSSR you must complete each of the tabs on the left for each of the communications included in the program. Once all the communications have been completed and activated the status beside each tab as well as the CSSR logo will turn to a green check mark.',
        footer: 'For additional information on the CSSR program, please click <a href="/content/static/cm/programs/cssr/CSSR_en.pdf" target="_new"><b>here</b></a> to view the CSSR program overview.',
        imgurl: '/resources/images/cssr/cssr_wheel_en.png'
    }
    const cssrDoc_fr = {
        header: 'Fidélisation du Client aux Ventes + au ServiceAprès-vente',
        body: "Le programme FCVS est un programme de communication de fidélisation dynamique et l'un des éléments clés du nouveau Programme d'excellence des services de détail de GM. <br/>" + 
            "Pour vous conformer au programme d'excellence des services de détail et obtenir l'état vert pour le programme FCVS, vous devez remplir chaque onglet de gauche pour chacune des communications incluses dans le programme. Lorsque toutes les communications auront été complétées et activées, l'état vert à côté de chaque onglet et le logo du programme FCVS afficheront un crochet vert.",
        footer: 'Pour en savoir plus sur le programme FCVS, veuillez cliquer <a href="/content/static/cm/programs/cssr/CSSR_fr.pdf" target="_new"><b>ici</b></a> pour afficher l\'aperçu.',
        imgurl: '/resources/images/cssr/cssr_wheel_fr.png'
    }
    

    var renderMenuList = function () {
        $('#menu-list').empty();
        var sHtml = "";

        for (var i = 0, len = summaryData.length; i < len; i++) {
            sHtml += '<li>';
            sHtml += '<a id="menu-' + summaryData[i].programID + '" href="#Programs" onclick="showPanes(' + i + '); return false" title="' + summaryData[i].title + '">';

            if (summaryData[i].status === true) {
                //sHtml += '<i class="fa fa-lg fa-fw fa-check-circle check"></i>';
                sHtml += '<span class="fa-lg fa-stack" style="margin-left: -7px">';
                sHtml += '  <i class="fa fa-circle fa-stack-1x check" style="font-size: 1.2em"></i>';
                sHtml += '  <i class="fa fa-check fa-stack-1x fa-inverse" style="font-size: .7em"></i>';
                sHtml += '</span>';
            }
            else {
                //sHtml += '<i class="fa fa-lg fa-fw fa-exclamation-circle uncheck"></i>';
                sHtml += '<span class="fa-lg fa-stack" style="margin-left: -7px">';
                sHtml += '  <i class="fa fa-circle fa-stack-1x uncheck" style="font-size: 1.2em"></i>';
                sHtml += '  <i class="fa fa-exclamation fa-stack-1x fa-inverse" style="font-size: .7em"></i>';
                sHtml += '</span>';
            }
            sHtml += '<span class="menu-item-parent">' + summaryData[i].title + '</span></a>';

            if (summaryData[i].programID === 208) {
                summaryData[i].checks[1].code = "C";
                summaryData[i].checks[1].title = "Creative";
            }
        }

        $('#menu-list').html(sHtml);
    }

    var renderTabs = function (data) {
        var sHtml = "";
        for (var i = 0, len = data.length; i < len; i++) {
            sHtml = createTabsHtml(data[i]);
            $('#pane-' + i + ' > .row-tabs').html(sHtml);
        }
    }

    var renderPaneContent = function () {
        $('#control-panes').empty();
        var sHtml = "";

        for (var i = 0, len = summaryData.length; i < len; i++) {
            sHtml += '<div id="pane-' + i + '" class="pane-content" data-pid="' + summaryData[i].programID + '">';
            sHtml += '  <div class="header">';
            sHtml += '    <span class="fa-lg fa-stack" style="height: 1.3em; line-height: 1.3em">';
            sHtml += '      <i class="fa fa-circle fa-stack-1x uncheck" style="font-size: 1.5em"></i>';
            sHtml += '      <i class="fa fa-exclamation fa-stack-1x fa-inverse" style="font-size: 1em"></i>';
            sHtml += '    </span>';
            sHtml += '    <span>' + summaryData[i].title + '</span>';
            sHtml += '    <button class="btn activate-button text-capitalize" onclick="cssr.toggleActive(' + summaryData[i].programID + '); return false;">' + myVar.ms_activate + '</button>';
            sHtml += '  </div>';
            sHtml += '  <div class="row-tabs"></div>';
            sHtml += '  <div class="offer-card col-full">';
            sHtml += '    <div class="cardBar"></div>';
            for (var j = 0, l = summaryData[i].checks.length; j < l; j++) {
                if (summaryData[i].checks[j].code === "T") {
                    sHtml += '    <div class="dvTControl-' + i + ' dviFrame" style="margin-left: 20px">';
                    sHtml += '      <iframe id="ifTarget-' + i + '" src="" data-src="' + defaultUrls.target + '?pid=' + summaryData[i].programID + '" width="100%" height="768" scrolling="auto" frameborder="0"></iframe>';
                    sHtml += '    </div>';
                }
                if (summaryData[i].checks[j].code === "O") {
                    sHtml += '    <div class="dvOControl-' + i + ' dviFrame" style="margin-left: 20px; display: none">'
                    sHtml += '      <iframe id="ifOffers-' + i + '" src="" data-src="' + defaultUrls.offer + '?pid=' + summaryData[i].programID + '" width="100%" height="768" scrolling="auto" frameborder="0"></iframe>';
                    sHtml += '    </div>';
                }
                if (summaryData[i].checks[j].code === "C") {
                    sHtml += '    <div class="dvCControl-' + i + ' dviFrame" style="margin-left: 20px; display: none">';
                    sHtml += '      <iframe id="ifCreative-' + i + '" src="" data-src="' + defaultUrls.creative + '?pid=' + summaryData[i].programID + '" width="100%" height="768" scrolling="auto" frameborder="0"></iframe>';
                    sHtml += '    </div>';
                }
                if (summaryData[i].checks[j].code === "S") {
                    sHtml += '    <div class="dvSControl-' + i + ' dviFrame" style="margin-left: 20px; display: none">';
                    sHtml += '      <iframe id="ifServices-' + i + '" src="" data-src="' + defaultUrls.service + '?pid=' + summaryData[i].programID + '" width="100%" height="768" scrolling="auto" frameborder="0"></iframe>';
                    sHtml += '    </div>';
                }
            }
            sHtml += '  </div>';            
            sHtml += '</div>';
        }

        $('#control-panes').html(sHtml);
    }

    var renderDescCards = function (data, index) {
        var item = '<div class="listBox">';
        item += '<div class="listBox-headbar"></div>';
        item += '<div class="listTitle" onClick="showPanes(' + index + ')" style="cursor: pointer">';
        item += '  <div class="flagBlock"><i class="fa fa-lg fa-fw fa-flag"></i></div>';
        item += '  <div class="listTitle-text">' + data.Title + '</div>';
        item += '</div>';
        item += '<div class="valueBox customScrollBar">' + data.Description + '</div>';
        item += '<div class="listBox-bottomrow">';
        item += '  <div>';
        item += '    <span><b>Format(s): </b>' + data.Format + '</span>';
        item += '  </div>';
        item += '  <div>';
        item += '    <span><b>Cadence: &nbsp;&nbsp;</b>' + data.Cadence + '</span>';
        item += '  </div>';
        item += '</div>';
        item += '</div>';
        $('#cardsview > .col-full').append(item);
    }

    var createTabsHtml = function(data) {
        var sHtml = "";

        for (var i = 0, len = data.checks.length; i < len; i++) {
            var index = 0;
            switch (data.checks[i].code) {
                case "T": index = 1; break;
                case "O": index = 2; break;
                case "C": index = 3; break;
                case "S": index = 4; 
            }
            sHtml += '<div class="tab-option" data-opt="' + index + '">';
            if (data.checks[i].status)
                sHtml += '<i class="fa fa-check-circle check"></i>';
            else
                sHtml += '<i class="fa fa-exclamation-circle uncheck"></i>';
            sHtml += '  <span>' +  data.checks[i].title + '</span>';
            sHtml += '</div>';
        }

        return sHtml;
    }

    var toggleActive = function (pid) {
        toggleProgramStatus(pid);
    }

    var isAllChecked = function (index) {
        var allchecked = true;
        for (var i = 0, l = summaryData[index].checks.length; i < l; i++) {
            if (summaryData[index].checks[i].status === false) {
                allchecked = false;
                break;
            }
        }
        return allchecked;
    }

    var doAllMenuChecked = function () {
        var isAllMenuChecked = true;
        for (var i = 0, l = summaryData.length; i < l; i++) {
            if (summaryData[i].status === false) {
                isAllMenuChecked = false;
                break;
            }
        }
        
        if (isAllMenuChecked) {
            //$('#cssr-logo > i').removeClass("fa-exclamation-circle uncheck").addClass("fa-check-circle check");
            $('#cssr-logo i.fa-circle').removeClass("uncheck").addClass("check");
            $('#cssr-logo i.fa-exclamation').removeClass("fa-exclamation").addClass("fa-check");
        }
        else {
            //$('#cssr-logo > i').removeClass("fa-check-circle check").addClass("fa-exclamation-circle uncheck");
            $('#cssr-logo i.fa-circle').removeClass("check").addClass("uncheck");
            $('#cssr-logo i.fa-check').removeClass("fa-check").addClass("fa-exclamation");
        }
    }

    var enableStatusButton = function (index) {
        if (index !== -1) {
            if (isAllChecked(index))
                $('#pane-' + cssr.optComb.menuIndex + ' > .header > button').prop("disabled", false).css("cursor", "pointer");
            else {
                $('#pane-' + cssr.optComb.menuIndex + ' > .header i.fa-circle').removeClass("check").addClass("uncheck");
            	$('#pane-' + cssr.optComb.menuIndex + ' > .header i.fa-check').removeClass("fa-check").addClass("fa-exclamation");
            	$('#pane-' + cssr.optComb.menuIndex + ' > .header > button').prop("disabled", true).css("cursor", "default").text(myVar.ms_activate);
            	$('#pane-' + cssr.optComb.menuIndex + ' > .header > button').css({ "background-color": "#F44336" });

                $('#menu-' + summaryData[index].programID + ' i.fa-circle').removeClass("check").addClass("uncheck");
            	$('#menu-' + summaryData[index].programID + ' i.fa-check').removeClass("fa-check").addClass("fa-exclamation");
            }
        }
    }

    var initLayout = function () {
        if (summaryData.length > 0) {
            renderPaneContent();
            renderTabs(summaryData);

            $('.tab-option').click(function () {
                $('#pane-' + cssr.optComb.menuIndex + ' > .row-tabs > .tab-option').css({ "border-bottom": "0" });

                $(this).css({ "border-bottom": "3px solid #01A453" });
                cssr.optComb.optIndex = parseInt($(this).data('opt'));

                //console.log('menuIndex = ' + cssr.optComb.menuIndex + ', optIndex = ' + cssr.optComb.optIndex);
                switch (cssr.optComb.optIndex) {
                    case 1:
                        $('#pane-' + cssr.optComb.menuIndex + ' > .offer-card > .dviFrame').hide();

                        if ($('#ifTarget-' + cssr.optComb.menuIndex).attr('src') === "") {
                            $('section.loading').show();
                            $('#ifTarget-' + cssr.optComb.menuIndex).attr('src', function () {
                                return $(this).data('src');
                            });

                            $('#ifTarget-' + cssr.optComb.menuIndex).load(function () {
                                var $frame = $(this);
                                var doc = $frame[0].contentWindow.document;
                                var $form = $('form', doc);
                                var h = $form.height();
                                $frame.height(h + 40);
                                var $fp = $frame.parent();
                                $fp.css({ height: (h + 40).toString() + "px" });
                                $fp.parent().css({ visibility: "visible" });

                                var $actButton = $('#tseMain_rbtnComponentStatus_input', doc);
                                var abText = ($actButton.val()) ? $actButton.val() : "";
                                var i = changeTabStatus(summaryData[cssr.optComb.menuIndex].programID, "T", abText.indexOf("Complet") > -1 ? true : false);
                                enableStatusButton(i);  // all tabs in program are true, enable status button

                                $('section.loading').hide();
                            });
                        }

                        $('.dvTControl-' + cssr.optComb.menuIndex).show();
                        break;
                    case 2:
                        $('#pane-' + cssr.optComb.menuIndex + ' > .offer-card > .dviFrame').hide();

                        if ($('#ifOffers-' + cssr.optComb.menuIndex).attr('src') === "") {
                            $('section.loading').show();
                            $('#ifOffers-' + cssr.optComb.menuIndex).attr('src', function () {
                                return $(this).data('src');
                            });

                            $('#ifOffers-' + cssr.optComb.menuIndex).load(function () {
                                var $frame = $(this);
                                var doc = $frame[0].contentWindow.document;
                                var $form = $('form', doc);
                                var h = $form.height();
                                $frame.height(h + 40);
                                var $fp = $frame.parent();
                                $fp.css({ height: (h + 40).toString() + "px" });
                                $fp.parent().css({ visibility: "visible" });

                                var $actButton = $('#oseMain_rbtnComponentStatus_input', doc);
                                var abText = ($actButton.val()) ? $actButton.val() : "";
                                var i = changeTabStatus(summaryData[cssr.optComb.menuIndex].programID, "O", abText.indexOf("Complet") > -1 ? true : false);
                                enableStatusButton(i);  

                                $('section.loading').hide();
                            });
                        }

                        $('.dvOControl-' + cssr.optComb.menuIndex).show();
                        break;
                    case 3:
                        $('#pane-' + cssr.optComb.menuIndex + ' > .offer-card > .dviFrame').hide();

                        if ($('#ifCreative-' + cssr.optComb.menuIndex).attr('src') === "") {
                            $('section.loading').show();
                            $('#ifCreative-' + cssr.optComb.menuIndex).attr('src', function () {
                                return $(this).data('src');
                            });

                            $('#ifCreative-' + cssr.optComb.menuIndex).load(function () {
                                var $frame = $(this);
                                var doc = $frame[0].contentWindow.document;
                                var $form = $('form', doc);
                                var h = $form.height();
                                $frame.height(h + 40);
                                var $fp = $frame.parent();
                                $fp.css({ height: (h + 40).toString() + "px" });
                                $fp.parent().css({ visibility: "visible" });

                                var $actButton = $('#cseMain_rbtnComponentStatus_input', doc);
                                var abText = ($actButton.val()) ? $actButton.val() : "";
                                var i = changeTabStatus(summaryData[cssr.optComb.menuIndex].programID, "C", abText.indexOf("Complet") > -1 ? true : false);
                                enableStatusButton(i);

                                $('section.loading').hide();
                            });
                        }

                        $('.dvCControl-' + cssr.optComb.menuIndex).show();
                        break;
                    case 4:
                        $('#pane-' + cssr.optComb.menuIndex + ' > .offer-card > .dviFrame').hide();

                        if ($('#ifServices-' + cssr.optComb.menuIndex).attr('src') === "") {
                            $('section.loading').show();
                            $('#ifServices-' + cssr.optComb.menuIndex).attr('src', function () {
                                return $(this).data('src');
                            });

                            $('#ifServices-' + cssr.optComb.menuIndex).load(function () {
                                var $frame = $(this);
                                var doc = $frame[0].contentWindow.document;
                                var $form = $('form', doc);
                                var h = $form.height();
                                $frame.height(h + 40);
                                var $fp = $frame.parent();
                                $fp.css({ height: (h + 40).toString() + "px" });
                                $fp.parent().css({ visibility: "visible" });

                                var $actButton = $('#sseMain_rbtnComponentStatus_input', doc);
                                var abText = ($actButton.val()) ? $actButton.val() : "";
                                var i = changeTabStatus(summaryData[cssr.optComb.menuIndex].programID, "S", abText.indexOf("Complet") > -1 ? true : false);
                                enableStatusButton(i);

                                $('section.loading').hide();
                            });
                        }

                        $('.dvSControl-' + cssr.optComb.menuIndex).show();
                        break;
                }

            });

            $('#cardsview').show();
        }
    }

    var initSideMenu = function () {
        renderMenuList();
    }

    var initCssrDocView = function () {
        if (myVar.language === 'fr') {
            $('#cssrDocument > .header > span').text(cssrDoc_fr.header);
            $('#cssrDocument > .col-full > div#cssrbody').html(cssrDoc_fr.body);
            $('#cssrDocument > .col-full > div#cssrimg > img').attr('src', cssrDoc_fr.imgurl);
            $('#cssrDocument > .col-full > div#cssrfooter').html(cssrDoc_fr.footer);
        }
    }

    var initLog4Javascript = function () {
        log = log4javascript.getLogger("myLogger");

        var ajaxAppender = new log4javascript.AjaxAppender("//ec2-35-167-23-4.us-west-2.compute.amazonaws.com/alert/CssrLog.php");
        ajaxAppender.setThreshold(log4javascript.Level.ALL);
        var ajaxLayout = new log4javascript.PatternLayout("%d{yyyy-MM-dd HH:mm:ss,SSS} [%p] - %m%n");
        ajaxAppender.setLayout(ajaxLayout);
        ajaxAppender.addHeader("Content-Type", "application/json");
        log.addAppender(ajaxAppender);
    }

    var getRequestIP = function () {
        $.getJSON('//geoip.nekudo.com/api', function (data) {
            var ipInfo = myVar.dealerID + ":" + myVar.userID + " access from [" + data.ip + "] " + data.city + ", " + data.country.name + ". Location is " + data.location.latitude + ", " + data.location.longitude;
            log.debug(ipInfo);
        });
    }

    /*********************
      Web Services 
     *********************/
    var testDemo = function () {
        //var answer = confirm(myVar.ms_deleteDraftConfirm);

        //if (answer) {
            $('section.loading').show();
            var wsObj = new DealerDirect.WebServices.CampaignManagement.CSSR();

            wsObj.testDemo(
                myVar.dealerID,
                myVar.userID,
                function (msg) {
                    if (msg !== "-1" && msg !== "") {  // usually is 'Y'
                        //console.log(msg);
                        toastr["success"](msg);
                    }

                    $('section.loading').hide();
                    console.log("call web service successfully");
                },
                function (error) {
                    $('section.loading').hide();
                    console.log("Fail to call web service");
                }, null);
        //}
    }

    var getSummary = function () {
        $('section.loading').show();
        var wsObj = new DealerDirect.WebServices.CampaignManagement.CSSR();

        wsObj.getSummary(
            myVar.dealerID,
            myVar.userID,
            function (msg) {
                if (msg !== "-1" && msg !== "") {
                    var data = JSON.parse(msg);
                    console.log("programs = " + data.length);
                    console.log(JSON.stringify(data, null, 2));
                    $('#cardsview > .col-full').empty();
                    for (var i = 0, len = data.length; i < len; i++) {
                        var s = $.extend(true, {}, summary);
                        s.programID = data[i].ProgramID;
                        s.subcampaignID = data[i].SubCampaignID;
                        s.status = (data[i].Status === "Active" || data[i].Status === "Subscribed" || data[i].Status === "Actif" || data[i].State === "Abonné") ? true : false;
                        if (s.programID > 300) data[i].Title += " (2017)";
                        s.title = data[i].Title;
                        s.cadence = data[i].Cadence;
                        s.fortmats = data[i].Format;
                        for (var j = 0, l = data[i].Checks.length; j < l; j++) {
                            if (data[i].Checks[j].Status === "Check")
                                s.checks.push({ code: data[i].Checks[j].ComponentCode, status: true, title: data[i].Checks[j].Title });
                            else
                                s.checks.push({ code: data[i].Checks[j].ComponentCode, status: false, title: data[i].Checks[j].Title });
                        }

                        summaryData.push(s);

                        renderDescCards(data[i], i);
                    }


                    if (summaryData.length > 0) {
                        var d1 = window.btoa(dealerdirect.encode_utf8(JSON.stringify(summaryData)));
                        sessionStorage.setItem("_s", d1);
                        initSideMenu();
                        initLayout();
                        doAllMenuChecked();
                    }
                }

                $('section.loading').hide();
                console.log("Successfully get summary");
            },
            function (error) {
                $('section.loading').hide();
                console.log("Fail to get summary");
            }, null);
    }

    var toggleProgramStatus = function (pid) {
        $('section.loading').show();
        var wsObj = new DealerDirect.WebServices.CampaignManagement.CSSR();

        wsObj.toggleProgramStatus(
            myVar.dealerID,
            myVar.userID,
            pid,
            function (msg) {
                $('section.loading').hide();
                console.log("Toggle program status successfully");

                if (msg !== "-1" && msg !== "") {
                    for (var i = 0, len = summaryData.length; i < len; i++) {
                        if (summaryData[i].programID === pid) {
                            summaryData[i].status = msg;
                            if (summaryData[i].status === true) {
                                //$('#pane-' + i + ' > .header > i').removeClass("fa-exclamation-circle uncheck").addClass("fa-check-circle check");
                                $('#pane-' + i + ' > .header i.fa-circle').removeClass("uncheck").addClass("check");
            					$('#pane-' + i + ' > .header i.fa-exclamation').removeClass("fa-exclamation").addClass("fa-check");
            					$('#pane-' + i + ' > .header > button').text(myVar.ms_deactivate);
            					$('#pane-' + i + ' > .header > button').css({ "background-color": "#01A453" });
                                //$('#menu-' + pid + ' > i').removeClass("fa-exclamation-circle uncheck").addClass("fa-check-circle check");
                                $('#menu-' + pid + ' i.fa-circle').removeClass("uncheck").addClass("check");
            					$('#menu-' + pid + ' i.fa-exclamation').removeClass("fa-exclamation").addClass("fa-check");
                            }
                            else {
                                //$('#pane-' + i + ' > .header > i').removeClass("fa-check-circle check").addClass("fa-exclamation-circle uncheck");
                                $('#pane-' + i + ' > .header i.fa-circle').removeClass("check").addClass("uncheck");
            					$('#pane-' + i + ' > .header i.fa-check').removeClass("fa-check").addClass("fa-exclamation");
            					$('#pane-' + i + ' > .header > button').text(myVar.ms_activate);
            					$('#pane-' + i + ' > .header > button').css({ "background-color": "#F44336" });
                                //$('#menu-' + pid + ' > i').removeClass("fa-check-circle check").addClass("fa-exclamation-circle uncheck");
                                $('#menu-' + pid + ' i.fa-circle').removeClass("check").addClass("uncheck");
            					$('#menu-' + pid + ' i.fa-check').removeClass("fa-check").addClass("fa-exclamation");
                            }

                            break;
                        }
                    }

                    doAllMenuChecked();
                }
            },
            function (error) {
                $('section.loading').hide();
                console.log("Fail to toggle program status");
            }, null);

        return -1;
    }

    return {
        optComb: {
            menuIndex: 1,
            optIndex: 1
        },
        toggleActive: toggleActive,
        initLayout: initLayout,
        initSideMenu: initSideMenu,
        initCssrDocView: initCssrDocView,
        isAllChecked: isAllChecked,
        enableStatusButton: enableStatusButton,
        testDemo: testDemo,
        getSummary: getSummary,
        initLog4Javascript: initLog4Javascript,
        getRequestIP: getRequestIP
    };
})();

$(document).ready(function () {
    backToTop();

    cssr.initLog4Javascript();
    cssr.getRequestIP();

    cssr.initCssrDocView();
    //if (summaryData.length > 0) {
    //    cssr.initSideMenu();
    //    cssr.initLayout();
    //}

});

function showPanes(index) {
    cssr.optComb.menuIndex = index;

    if (summaryData.length > 0) {
        if (summaryData[index].status === true) {
            $('#pane-' + index + ' > .header i.fa-circle').removeClass("uncheck").addClass("check");
            $('#pane-' + index + ' > .header i.fa-exclamation').removeClass("fa-exclamation").addClass("fa-check");
            $('#pane-' + index + ' > .header > button').prop("disabled", false).css("cursor", "pointer").text(myVar.ms_deactivate);
            $('#pane-' + index + ' > .header > button').css({ "background-color": "#01A453" });

            $('#menu-' + summaryData[index].programID + ' i.fa-circle').removeClass("uncheck").addClass("check");
            $('#menu-' + summaryData[index].programID + ' i.fa-exclamation').removeClass("fa-exclamation").addClass("fa-check");
        }
        else {
            $('#pane-' + index + ' > .header i.fa-circle').removeClass("check").addClass("uncheck");
            $('#pane-' + index + ' > .header i.fa-check').removeClass("fa-check").addClass("fa-exclamation");
            $('#pane-' + index + ' > .header > button').prop("disabled", true).css("cursor", "default").text(myVar.ms_activate);
            $('#pane-' + index + ' > .header > button').css({ "background-color": "#F44336" });

            $('#menu-' + summaryData[index].programID + ' i.fa-circle').removeClass("check").addClass("uncheck");
            $('#menu-' + summaryData[index].programID + ' i.fa-check').removeClass("fa-check").addClass("fa-exclamation");

            if (cssr.isAllChecked(index)) $('#pane-' + index + ' > .header > button').prop("disabled", false).css("cursor", "pointer");
        }
    }

    $('#left-panel > nav > ul li > a > span').css({ "border-bottom": "0" });
    $('#menu-' + summaryData[index].programID + ' > span:nth-child(2)').css({ "border-bottom": "2px solid #FFF" });

    if ($('#ifTarget-' + index).attr('src') === "") {
        $('section.loading').show();

        $('#pane-' + index + ' > .row-tabs > .tab-option:first-child').css({ "border-bottom": "3px solid #01A453" });

        $('#ifTarget-' + index).attr('src', function () {
            return $(this).data('src');
        });

        $('#ifTarget-' + index).load(function () {
            var $frame = $(this);
            var doc = $frame[0].contentWindow.document;
            var $form = $('form', doc);
            var h = $form.height();
            $frame.height(h + 40);
            var $fp = $frame.parent();
            $fp.css({ height: (h + 40).toString() + "px" });
            $fp.parent().css({ visibility: "visible" });

            var $actButton = $('#tseMain_rbtnComponentStatus_input', doc);
            var abText = ($actButton.val()) ? $actButton.val() : "";
            var i = changeTabStatus(summaryData[index].programID, "T", abText.indexOf("Complet") > -1 ? true : false);
            cssr.enableStatusButton(i);

            $('section.loading').hide();
        });
    }
     
    $('.pane-content').hide();
    $('#pane-' + index).show();

    return false;
}

function changeTabStatus(pid, code, status) {
    for (var i = 0, l = summaryData.length; i < l; i++) {
        if (summaryData[i].programID === pid) {
            for (var j = 0, ll = summaryData[i].checks.length; j < ll; j++) {
                if (summaryData[i].checks[j].code === code) {
                    summaryData[i].checks[j].status = status;
                    //console.log(summaryData[i].programID + ", " + summaryData[i].checks[j].code + ", " + summaryData[i].checks[j].status);
                    if (status === true)
                        $('#pane-' + cssr.optComb.menuIndex + ' .tab-option[data-opt=' + cssr.optComb.optIndex + '] > i').removeClass("fa-exclamation-circle uncheck").addClass("fa-check-circle check");
                    else
                        $('#pane-' + cssr.optComb.menuIndex + ' .tab-option[data-opt=' + cssr.optComb.optIndex + '] > i').removeClass("fa-check-circle check").addClass("fa-exclamation-circle uncheck");

                    return i;
                }
            }
        }
    }
    return -1;
}

function backToTop() {
    var offset = 70;
    var duration = 500;
    $('#master-container').scroll(function () {
        if ($(this).scrollTop() > offset) {
            $('.back-to-top').fadeIn(duration);
        } else {
            $('.back-to-top').fadeOut(duration);
        }
    });

    $('.back-to-top').click(function (event) {
        $('#master-container').animate({ scrollTop: 0 }, duration);
        return false;
    })
}


