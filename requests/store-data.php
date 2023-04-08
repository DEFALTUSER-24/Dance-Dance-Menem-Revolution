<?php

    //Checks if POST request contains specified values.
    if (!request::PostHasKeys("name", "score", "level")) {
        request::EndWithError("Solicitud POST inválida, faltan datos.");
    }

    //Connect to database
    database::Connect();

    //Save escaped values to prevent SQL Injection)
    $username = database::Escape($_POST["name"]);
    $score = intval(database::Escape($_POST["score"]));
    $level = intval(database::Escape($_POST["level"]));

    //Send changes to database
    database::Query("INSERT INTO users_score (name, user_score, level) VALUES ('" . $username . "', " . $score . " " . $level . ")");

    //If no changes were made, end with error.
    if (!database::WasAffected()) {
        request::EndWithError("Query SQL inválida. No se agregó el puntaje.");
    }

    request::End(true);