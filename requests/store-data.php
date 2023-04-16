<?php

    //Checks if POST request contains specified values.
    if (!request::PostHasKeys("name", "score", "level")) {
        request::EndWithError("Solicitud POST inválida, faltan datos.");
    }

    //Connect to database
    database::Connect();

    function clamp($current, $min, $max) {
        return max($min, min($max, $current));
    }

    //Save escaped values to prevent SQL Injection)
    $username = trim(
        substr(database::Escape($_POST["name"]), 0, 20)
    );

    //Level should be taken from POST but for now we're using 1 by default.
    $level = 1; //intval(database::Escape($_POST["level"]))

    $score = clamp(
        intval(database::Escape($_POST["score"])),
        -99999, //min score
        999999 //max score
    );

    //If username is invalid, throw error
    if ($username == "") {
        request::EndWithError("El nombre de usuario no puede estar vacío.");
    }

    //Send changes to database
    database::Query("INSERT INTO users_score (name, user_score, level) VALUES ('" . $username . "', " . $score . ", " . $level . ")");

    //If no changes were made, end with error.
    if (!database::WasAffected()) {
        request::EndWithError("Query SQL inválida. No se agregó el puntaje.");
    }

    request::End(true);