<?php

// this converts a FileNumber Index to a DatPath

$model_id = 16;

$FileNumber = 16912;

$VTableFile = "E:\SquareEnix\SquareEnix\FINAL FANTASY XI\VTABLE.DAT";
$FTableFile = "E:\SquareEnix\SquareEnix\FINAL FANTASY XI\FTABLE.DAT";

$i = 1;

if (file_exists($VTableFile) && file_exists($FTableFile)) {
    try {
        $VBR = fopen($VTableFile, 'rb');

        if ($FileNumber < filesize($VTableFile)) {
            fseek($VBR, $FileNumber, SEEK_SET);
            
            $FBR = fopen($FTableFile, 'rb');
                
                fseek($FBR, 2 * $FileNumber, SEEK_SET);

                print_r([
                    unpack("v", fread($FBR, 2)),
                    unpack("v", fread($FBR, 4)),
                    unpack("v", fread($FBR, 8)),
                    unpack("v", fread($FBR, 10))
                ]);

                $pack = unpack("v", fread($FBR, 2));
                $filedir = $pack[1];

                $app = ($i - 1) & 0xff;
                $dir = (int)($filedir / 0x80);
                $file = (int)($filedir % 0x80);

                $dat = "ROM{$app}\\{$dir}\\{$file}.DAT";

                print_r([ $pack, $filedir, $app, $dir, $file, $dat ]);


                fclose($FBR);
                return true;
        } else {
            print_r("file number too big");
        }

        fclose($VBR);
    }
    catch (Exception $e) {}
}