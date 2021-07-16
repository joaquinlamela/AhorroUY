package com.ahorrouy.ui.helpers

import android.view.animation.Animation
import android.view.animation.BounceInterpolator
import android.view.animation.ScaleAnimation
import android.widget.Button
import android.widget.TextView
import android.widget.ToggleButton

class AdapterHelper {
    companion object Factory {

        fun setUpFavStarAnimation(favoriteButton: ToggleButton) {
            favoriteButton.setOnCheckedChangeListener { p0, _ ->
                p0?.startAnimation(
                    FavAnimation.getBounceInterpolator(
                        420
                    )
                )
            }
        }

        fun setUpOnAddCartAnimation(recyclerView: TextView) {
            recyclerView.startAnimation(FavAnimation.getBounceInterpolator(64))
        }

        fun setUpPlusMinusButtons(btnAdd: Button, btnMinus: Button, txtAmount: TextView) {
            btnAdd.setOnClickListener {
                var amount = Integer.parseInt(txtAmount.text.toString())
                amount += 1
                txtAmount.text = amount.toString()
                btnMinus.isEnabled = true
            }
            btnMinus.setOnClickListener {
                var amount = Integer.parseInt(txtAmount.text.toString())
                if (amount == 1) {
                    btnMinus.isEnabled = false
                } else {
                    amount -= 1
                    txtAmount.text = amount.toString()
                }
            }
        }
    }

    object FavAnimation {
        fun getBounceInterpolator(time: Long): ScaleAnimation {
            val favAnimation = ScaleAnimation(
                0.7f, 1.0f, 0.7f, 1.0f,
                Animation.RELATIVE_TO_SELF, 0.7f, Animation.RELATIVE_TO_SELF, 0.7f
            )
            favAnimation.duration = time
            favAnimation.interpolator = BounceInterpolator()
            return favAnimation
        }

    }
}