<?php

    class request
    {
        /**
         * End request and return JSON.
         *
         * @param bool $task_completed
         * @param array $data
         * @return void
         */
        public static function End(bool $task_completed, array $data = array())
        {
            database::CloseConnection();

            $jsonData = array();
            $jsonData['success'] = $task_completed;
            $jsonData['data'] =  $data;
            header('Content-type: application/json; charset=utf-8');
            echo json_encode($jsonData, JSON_UNESCAPED_UNICODE); //JSON_FORCE_OBJECT
            exit;
        }

        /**
         * End request with data only (no "success").
         *
         * @param array $data
         * @return void
         */
        public static function EndWithData(array $data = array())
        {
            database::CloseConnection();

            $jsonData = array();
            $jsonData['data'] = $data;

            header('Content-type: application/json; charset=utf-8');
            echo json_encode($jsonData, JSON_UNESCAPED_UNICODE); //JSON_FORCE_OBJECT
            exit;
        }

        /**
         * End Request with a specified error
         *
         * @param string $error_description
         * @return void
         */
        public static function EndWithError(string $error_description)
        {
            self::End(false, array("error" => $error_description));
        }

        /**
         * Checks if $_GET contains "action" key which is used to do different things inside the system.
         *
         * @return bool
         */
        public static function IsValid(): bool
        {
            return array_key_exists("action", $_GET);
        }

        /**
         * Checks if request is POST type.
         *
         * @return bool
         */
        public static function IsPost(): bool
        {
            return $_SERVER['REQUEST_METHOD'] == 'POST';
        }

        /**
         * Checks if request is GET type.
         *
         * @return bool
         */
        public static function IsGet(): bool
        {
            return $_SERVER['REQUEST_METHOD'] == 'GET';
        }

        /**
         * Checks whole $_POST to see if it contains keys passed as parameters.
         * You can check unlimited keys with this method.
         *
         * @return bool
         */
        public static function PostHasKeys(): bool
        {
            $args = func_get_args();
            foreach ($args as $key) {
                if (!array_key_exists($key, $_POST)) {
                    return false;
                }
            }
            return true;
        }
    }