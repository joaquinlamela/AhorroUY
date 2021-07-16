package com.ahorrouy.ui.coupons.adapter

import android.os.Build
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.ImageView
import android.widget.TextView
import androidx.annotation.RequiresApi
import androidx.recyclerview.widget.DiffUtil
import androidx.recyclerview.widget.ListAdapter
import androidx.recyclerview.widget.RecyclerView
import com.ahorrouy.R
import com.ahorrouy.api.model.coupon.CouponGetResponse
import com.squareup.picasso.Picasso
import java.time.LocalDateTime
import java.time.temporal.ChronoUnit

class CouponsAdapter(private val onClick: (CouponGetResponse) -> Unit) :
    ListAdapter<CouponGetResponse, CouponsAdapter.CouponsViewHolder>(CouponDiffCallback) {

    class CouponsViewHolder(view: View, val onClick: (CouponGetResponse) -> Unit) :
        RecyclerView.ViewHolder(view) {
        private val txtValue: TextView = view.findViewById(R.id.txt_value)
        private val txtExpirationDate: TextView = view.findViewById(R.id.txt_expiration)
        private val btnViewQR: Button = view.findViewById(R.id.btn_ver_qr)
        private val txtExpired: TextView = view.findViewById(R.id.txt_expired)
        private val imgMarketLogo: ImageView = view.findViewById(R.id.coupon_image)

        private lateinit var currentCoupon: CouponGetResponse

        init {
            view.setOnClickListener {
                onClick(currentCoupon)
            }

            btnViewQR.setOnClickListener {
                onClick(currentCoupon)
            }
        }

        @RequiresApi(Build.VERSION_CODES.O)
        fun bind(coupon: CouponGetResponse) {
            currentCoupon = coupon

            val fromDate = LocalDateTime.now()
            val toDate = LocalDateTime.parse(coupon.Deadline)
            val days = ChronoUnit.DAYS.between(fromDate, toDate).toInt()
            val hours = ChronoUnit.HOURS.between(fromDate, toDate).toInt()
            val minutes = ChronoUnit.MINUTES.between(fromDate, toDate).toInt() % 60
            when {
                toDate < fromDate -> {
                    btnViewQR.visibility = View.INVISIBLE
                    txtExpirationDate.visibility = View.INVISIBLE
                    txtExpired.visibility = View.VISIBLE
                }
                days == 0 -> {
                    btnViewQR.visibility = View.VISIBLE
                    txtExpirationDate.visibility = View.VISIBLE
                    txtExpired.visibility = View.INVISIBLE
                    if (hours == 0) txtExpirationDate.text = "Expira en $minutes minutos"
                    else txtExpirationDate.text = "Expira en ${hours}h ${minutes}m"
                }
                else -> {
                    btnViewQR.visibility = View.VISIBLE
                    txtExpirationDate.visibility = View.VISIBLE
                    txtExpired.visibility = View.INVISIBLE
                    if (days == 1) txtExpirationDate.text = "Expira en 1 dia"
                    else txtExpirationDate.text = "Expira en $days dias"
                }
            }
            Picasso.get().load(coupon.MarketCouponLogoURL).into(imgMarketLogo)
            txtValue.text = "$" + coupon.Value
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): CouponsViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_coupon, parent, false)
        return CouponsViewHolder(view, onClick)
    }

    @RequiresApi(Build.VERSION_CODES.O)
    override fun onBindViewHolder(holder: CouponsViewHolder, position: Int) {
        val coupon = getItem(position)
        holder.bind(coupon)
    }
}

object CouponDiffCallback : DiffUtil.ItemCallback<CouponGetResponse>() {
    override fun areItemsTheSame(oldItem: CouponGetResponse, newItem: CouponGetResponse): Boolean {
        return oldItem == newItem
    }

    override fun areContentsTheSame(
        oldItem: CouponGetResponse,
        newItem: CouponGetResponse
    ): Boolean {
        return oldItem.Url == newItem.Url
    }
}