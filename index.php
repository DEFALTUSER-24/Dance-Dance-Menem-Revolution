<?php

    //Require specified files.
    require "settings/database.php";
    require "settings/request.php";

    //Checks if request is valid (if "action" was used as a $_GET parameter).
    if (!request::IsValid())
        request::EndWithError("Solicitud inválida.");

    $action = $_GET["action"];

    //Do different actions based on $_GET "action" value.
    switch ($action)
    {
        case "add-score":
        {
            if (!request::IsPost())
                request::EndWithError("Esta solicitud tiene que ser tipo POST.");

            require "requests/store-data.php";
            break;
        }
        case "get-scores":
        {
            if (!request::IsGet())
                request::EndWithError("Esta solicitud tiene que ser tipo GET.");

            require "requests/get-data.php";
            break;
        }
        default:
        {
            request::EndWithError("Acción inválida.");
        }
    }