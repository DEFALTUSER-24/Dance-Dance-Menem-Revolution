<?php

    class request
    {
        public static function End(bool $task_completed, array $data = array())
        {
            database::CloseConnection();

            $jsonData = array();
            $jsonData['success'] = $task_completed;
            $jsonData['data'] =  $data;
            header('Content-type: application/json; charset=utf-8');
            echo json_encode($jsonData, JSON_FORCE_OBJECT);
            exit;
        }

        public static function EndWithError(string $error_description)
        {
            self::End(false, array("error" => $error_description));
        }

        public static function IsValid(): bool
        {
            return array_key_exists("action", $_GET);
        }

        public static function IsPost(): bool
        {
            return $_SERVER['REQUEST_METHOD'] == 'POST';
        }

        public static function IsGet(): bool
        {
            return $_SERVER['REQUEST_METHOD'] == 'GET';
        }

        public static function PostHasKeys(): bool
        {
            $args = func_get_args();
            foreach ($args as $key) {
                if (!array_key_exists($key, $_POST) || empty($_POST[$key])) {
                    return false;
                }
            }
            return true;
        }
    }