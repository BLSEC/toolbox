<?php
// include 'session.php';   // uncomment once db is re-setup
// include 'conn.php':

?>

<html>
    <!-- <style>
        th, td {
            border: 1px solid;

        }
    </style> -->
    <center>
    <table>

        <tr>
            <th>Hostname</th>

            <th>IP Address</th>

            <th>OS</th>

            <th>Action</th>
        </tr>

        <?php
        $botQuery = $mysql->query("select * from targets");

        while($row = $botQuery->fetch_assoc())
        {
            $hostname = $row["hostname"];
            $operatingsystem = $row["operatingsystem"];
            $ipaddress = $row["ipaddress"];
            $action = "<a href='/manage.php?bot=" . $hostname . "'>Manage</a>";
            echo "<tr>";
            echo "<td>".$hostname."</td>";
            echo "<td>".$operatingsystem."</td>";
            echo "<td>".$ipaddress."</td>";
            echo "<td>".$action."</td>";
            echo "</tr>";
        }
        ?>


    </table>
</html>
