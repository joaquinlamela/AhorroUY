package com.ahorrouy.ui.coupons

import android.graphics.Bitmap
import android.graphics.Color
import android.os.Build
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.annotation.RequiresApi
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import androidx.fragment.app.viewModels
import androidx.lifecycle.Observer
import androidx.recyclerview.widget.RecyclerView
import com.ahorrouy.R
import com.ahorrouy.api.model.coupon.CouponGetResponse
import com.ahorrouy.repository.implementation.CouponRepository
import com.ahorrouy.ui.coupons.adapter.CouponsAdapter
import com.ahorrouy.ui.coupons.viewmodel.CouponsViewModel
import com.ahorrouy.ui.coupons.viewmodel.CouponsViewModelFactory
import com.ahorrouy.ui.user.viewmodel.UserViewModel
import com.google.android.material.snackbar.Snackbar
import com.google.zxing.BarcodeFormat
import com.google.zxing.qrcode.QRCodeWriter
import java.time.LocalDateTime

class CouponsFragment : Fragment() {

    private val userViewModel: UserViewModel by activityViewModels()
    private val couponsViewModel by viewModels<CouponsViewModel> {
        CouponsViewModelFactory(CouponRepository())
    }

    @RequiresApi(Build.VERSION_CODES.O)
    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val root = inflater.inflate(R.layout.fragment_coupons, container, false)

        val noCouponsTextView: TextView = root.findViewById(R.id.txt_noCoupons)

        val couponsAdapter = CouponsAdapter(::adapterOnClick)
        val recyclerView: RecyclerView = root.findViewById(R.id.recycler_view_coupons)
        recyclerView.adapter = couponsAdapter

        val token = userViewModel.currentUser.value!!.Token
        couponsViewModel.refresh(token)

        couponsViewModel.coupons.observe(viewLifecycleOwner, Observer { response ->
            if (response.isSuccessful) {
                val list = response.body() as MutableList<CouponGetResponse>
                val number = list.size
                if (number == 0) noCouponsTextView.visibility = View.VISIBLE
                list.sortBy { LocalDateTime.parse(it.Deadline) }
                list.sortBy { LocalDateTime.now() > LocalDateTime.parse(it.Deadline) }
                couponsAdapter.submitList(list)
            } else {
                var errorCode = response.raw().code.toString()
                if (response.headers()["ErrorCode"] != null)
                    errorCode = response.headers()["ErrorCode"] as String
                handleResponseError(errorCode, root)
            }
        })
        return root
    }

    @RequiresApi(Build.VERSION_CODES.O)
    private fun adapterOnClick(coupon: CouponGetResponse) {
        if (LocalDateTime.now() < LocalDateTime.parse(coupon.Deadline)) {
            val qrBitmap = generateQRImage(coupon)
            val dialog = QRCodeDialog(qrBitmap)
            val fragmentManager = parentFragmentManager
            dialog.show(fragmentManager, "QRDialogFragment")
        }
    }

    private fun generateQRImage(coupon: CouponGetResponse): Bitmap {
        val content = coupon.Url
        val writer = QRCodeWriter()
        val bitMatrix = writer.encode(content, BarcodeFormat.QR_CODE, 512, 512)
        val width = bitMatrix.width
        val height = bitMatrix.height
        val bitmap = Bitmap.createBitmap(width, height, Bitmap.Config.RGB_565)
        for (x in 0 until width) {
            for (y in 0 until height) {
                bitmap.setPixel(x, y, if (bitMatrix.get(x, y)) Color.BLACK else Color.WHITE)
            }
        }
        return bitmap
    }

    private fun handleResponseError(
        errorCode: String,
        view: View
    ) {
        var errorMsg: String = errorCode
        when (errorCode) {
            "500" -> errorMsg = "Error: No se pudo conectar al backend."
        }
        Snackbar.make(view, errorMsg, Snackbar.LENGTH_SHORT).setAction("OK") {}.show()
    }
}