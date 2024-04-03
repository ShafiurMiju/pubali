function changeDiv() {
        

        if (leftThumbFlag == 0) {
            //document.getElementById("fingerprintImageLeftblock").innerHTML = "Required!";
        }

        if (rightThumbFlag == 0) {
            //document.getElementById("fingerprintImageRightblock").innerHTML = "Required!";
        }
        /*
        $.ajax({
            
            url: '@Url.Action("ThumbConversionToWSQ", "ThumbImpressionVerification")',
            method: 'POST',
            //data: { 'NID': getNID, 'DOB': getDOB },
            success: function (data) {
                if (data) {

                }

                else {
                    //alert(data);
                }
            }
        });
        */
        
        //document.getElementById("resultDiv").style.visibility = "visible";
        

        var flag = [];
        flag.push(validateRequired($("#NID"), $("#NIDblock")))
        if ($("#NID").val()) flag.push(NIDCheck($("#NID"), $("#NIDblock")))
        flag.push(validateYear($("#birthDateID"), $("#birthDateIDblock")))
        //if(notCaptured==0)flag.push(1);

        if (!flag.includes(1)) {
            //notCaptured = 0;
            document.getElementById("resultDiv").style.visibility = "hidden";
            document.getElementById("sendBtn").disabled = true;
            document.getElementById("captureBtn").disabled = true;
            document.getElementById("matchingScoreID").innerHTML = "";
            const para = document.createElement("h1");
            para.setAttribute("id", "loading");
            para.setAttribute("class", "spinner-border text-info");
            para.setAttribute("role", "status");
            para.setAttribute("style", "width: 7rem; height: 7rem;");
            document.getElementById("diviframe").appendChild(para);




            var getNID = $('#NID').val();
            var getDOB = $('#birthDateID').val();
            var matchOrNotMatch;
            $.ajax({
                url: '@Url.Action("getThumbUploadURL", "ThumbImpressionVerification")',
                method: 'POST',
                data: { 'NID': getNID, 'DOB': getDOB },
                success: function (data) {
                    if (data) {
                        //alert(data);
                        matchOrNotMatch = JSON.parse(data);
                        if (matchOrNotMatch == "MATCH FOUND") {
                            document.getElementById("matchingScoreID").innerHTML = "MATHCED!";
                            document.getElementById("matchingScoreID").style.color = "green";
                        }
                        else if (matchOrNotMatch == "NO MATCH FOUND") {
                            document.getElementById("matchingScoreID").innerHTML = "UNMATCHED!";
                            document.getElementById("matchingScoreID").style.color = "red";
                        }
                        else {
                            document.getElementById("matchingScoreID").innerHTML = "ERROR!";
                            document.getElementById("matchingScoreID").style.color = "blue";
                        }
                    }

                    else {
                        //alert(data);
                    }
                }
            });

            

            $.ajax({
               url: '@Url.Action("GetCustomerDetailsFromECAPI", "ThumbImpressionVerification")',
               method: 'POST',
               data: { 'NID': getNID, 'DOB': getDOB },
               success: function (data) {
                   if (data) {

                       //alert("API calling is performing now!!!");

                       var myResult = JSON.parse(data);

                       if (myResult.ErrorMsg) {
                           alert(myResult.ErrorMsg);
                           window.location.reload();
                       }
                       else {
                           document.getElementById("loading").remove();
                           document.getElementById("resultDiv").style.visibility = "visible";
                           document.getElementById("sendBtn").disabled = false;
                           document.getElementById("captureBtn").disabled = false;
                           $('#imageFromEC').attr('src', myResult.NID_PHOTO);

                            
                           document.getElementById("nameBanglaFromEC").value = myResult.PERSON_NAME;
                           document.getElementById("NameEnglishFromEC").value = myResult.PERSON_ENAME;
                           document.getElementById("birthDateIDFromEC").value = myResult.NID_DOB;
                           document.getElementById("fatherNameFromEC").value = myResult.PERSON_FNAME;
                           document.getElementById("motherNameFromEC").value = myResult.PERSON_MNAME;
                           document.getElementById("spouseNameFromEC").value = myResult.PERSON_SPOUSE;

                           document.getElementById("divisionPresentFromEC").value = myResult.PreDivision;
                           document.getElementById("districtPresentFromEC").value = myResult.PreDistrict;
                           document.getElementById("rmoPresentFromEC").value = myResult.PreRMO;
                           document.getElementById("cityCorporationPresentFromEC").value = myResult.PreCityCorporation;
                           document.getElementById("upazilaPresentFromEC").value = myResult.PreUpazila;
                           document.getElementById("unionPresentFromEC").value = myResult.PreUnion;
                           document.getElementById("unionParishadPresentFromEC").value = myResult.PreUnionParishad;
                           document.getElementById("villagePresentFromEC").value = myResult.PreVillage;
                           document.getElementById("additionalVillagePresentFromEC").value = myResult.PreAdditionalVillage;
                           document.getElementById("homePresentFromEC").value = myResult.PreHome;
                           document.getElementById("postOfficePresentFromEC").value = myResult.PrePost_Office;
                           document.getElementById("regionPresentFromEC").value = myResult.PreRegion;

                           document.getElementById("divisionPermanentFromEC").value = myResult.PermDivision;
                           document.getElementById("districtPermanentFromEC").value = myResult.PermDistrict;
                           document.getElementById("rmoPermanentFromEC").value = myResult.PermRMO;
                           document.getElementById("cityCorporationPermanentFromEC").value = myResult.PermCityCorporation;
                           document.getElementById("upazilaPermanentFromEC").value = myResult.PermUpazila;
                           document.getElementById("unionPermanentFromEC").value = myResult.PermUnion;
                           document.getElementById("unionParishadPermanentFromEC").value = myResult.PermUnionParishad;
                           document.getElementById("villagePermanentFromEC").value = myResult.PermVillage;
                           document.getElementById("additionalVillagePermanentFromEC").value = myResult.PermAdditionalVillage;
                           document.getElementById("homePermanentFromEC").value = myResult.PermHome;
                           document.getElementById("postOfficePermanentFromEC").value = myResult.PermPost_Office;
                           document.getElementById("regionPermanentFromEC").value = myResult.PermRegion;
                       }
                   }

                   else {
                       //alert("paini");
                   }
               }
            });
        }
        
        if (true) { //!flag.includes(1) && leftThumbFlag != 0 && rightThumbFlag != 0





            // $("#accountNo").inputmask({ "mask": "9999" });

            //IMask(
            //    document.getElementById('accountNo'),
            //    {
            //        mask: '0000-000-000000'
            //    }
            //)

        }
        else {
            //alert("Left thumb or Right thumb is missing");
        }

    }
