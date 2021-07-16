package com.ahorrouy.ui.coupons

import android.app.Dialog
import android.graphics.Bitmap
import android.os.Bundle
import android.widget.ImageView
import androidx.appcompat.app.AlertDialog
import androidx.fragment.app.DialogFragment
import com.ahorrouy.R

class QRCodeDialog(private var qrImage: Bitmap) : DialogFragment() {

    override fun onCreateDialog(savedInstanceState: Bundle?): Dialog {
        val inflater = requireActivity().layoutInflater
        val view = inflater.inflate(R.layout.dialog_qrcode, null)
        val image = view.findViewById<ImageView>(R.id.img_qrcode)
        image.setImageBitmap(qrImage)

        return activity?.let {
            val builder = AlertDialog.Builder(it)
            builder.setView(view)
            builder.create()
        } ?: throw IllegalStateException("Activity cannot be null")
    }
}

