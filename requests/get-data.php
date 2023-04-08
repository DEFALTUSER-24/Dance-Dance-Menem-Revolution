<?php
    database::connect();

    $query_result = database::Query("SELECT * FROM users_score ORDER BY user_score DESC LIMIT 10");

    if (!database::Result($query_result))
        request::EndWithError("No se encontraron puntajes.");

    $query_data = array();

    while ($data = mysqli_fetch_assoc($query_result)) {
        $query_data[] = $data;
    }

    request::End(true, $query_data);