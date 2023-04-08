<?php

    if (!request::PostHasKeys("name", "score")) {
        request::EndWithError("Solicitud POST inválida, faltan datos.");
    }

    database::Connect();

    $username = database::Escape($_POST["name"]);
    $score = intval(database::Escape($_POST["score"]));

    database::Query("INSERT INTO users_score (name, user_score) VALUES ('" . $username . "', " . $score . ")");

    if (!database::WasAffected()) {
        request::EndWithError("Query SQL inválida. No se agregó el puntaje.");
    }

    request::End(true);