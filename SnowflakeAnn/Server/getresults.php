<?php
include 'conn.php';

if($_SERVER["REQUEST_METHOD"] == "POST" && isset($_POST["hostname"]) && isset($_POST["ip"]) && isset($_POST["result"]))
{
    $resultQuery = $mysql->prepare("update targets set commandresult=? where hostname=? and ipaddress=?");
    $resultQuery->bind_params("sss", $_POST["result"], $_POST["hostname"], $_POST["ip"]);
    $resultQuery->execute();
}


?>