<?php

    require "settings/database.php";
    require "settings/request.php";

    if (!request::IsValid())
        request::EndWithError("Solicitud inválida.");

    $action = $_GET["action"];

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