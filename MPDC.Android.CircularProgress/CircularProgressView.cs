using System;
using Android.Animation;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Java.Util.Jar;

namespace MPDC.Android.CircularProgress
{
    [Register("mpdc.android.CircularProgress")]
    public class CircularProgressView : View
    {
        public CircularProgressView(Context context) : this(context, null)
        {
        }

        public CircularProgressView(Context context, IAttributeSet attrs) : this(context, attrs, 0)
        {
        }

        public CircularProgressView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            default_text_size = sp2px(Resources, 18);
            min_size = (int)dp2px(Resources, 100);
            default_stroke_width = dp2px(Resources, 10);
            default_inner_bottom_text_size = sp2px(Resources, 18);

            TypedArray attributes = context.Theme.ObtainStyledAttributes(attrs, Resource.Styleable.DonutProgress, defStyleAttr, 0);
            initByAttributes(attributes);
            attributes.Recycle();

            initPainters();
        }


        Paint _finishedPaint;
        Paint _unfinishedPaint;
        Paint _innerCirclePaint;




        Paint _textPaint;
        Paint _innerBottomTextPaint;

        RectF _finishedOuterRect = new RectF();
        RectF _unfinishedOuterRect = new RectF();



        int attributeResourceId = 0;
        bool showText;
        float textSize;
        Color textColor;
        Color innerBottomTextColor;
        float progress = 0;
        int max;
        Color finishedStrokeColor;
        Color unfinishedStrokeColor;
        int startingDegree;
        float finishedStrokeWidth;
        float unfinishedStrokeWidth;
        Color innerBackgroundColor;
        String prefixText = "";
        String suffixText = "%";
        String text = null;
        float innerBottomTextSize;
        String innerBottomText;
        float innerBottomTextHeight;

        float default_stroke_width;
        Color default_finished_color = Color.Rgb(66, 145, 241);
        Color default_unfinished_color = Color.Rgb(204, 204, 204);
        Color default_text_color = Color.Rgb(66, 145, 241);
        Color default_inner_bottom_text_color = Color.Rgb(66, 145, 241);
        Color default_inner_background_color = Color.Transparent;
        int default_max = 100;
        int default_startingDegree = 0;
        float default_text_size;
        float default_inner_bottom_text_size;
        int min_size;


        private const String INSTANCE_STATE = "saved_instance";
        private const String INSTANCE_TEXT_COLOR = "text_color";
        private const String INSTANCE_TEXT_SIZE = "text_size";
        private const String INSTANCE_TEXT = "text";
        private const String INSTANCE_INNER_BOTTOM_TEXT_SIZE = "inner_bottom_text_size";
        private const String INSTANCE_INNER_BOTTOM_TEXT = "inner_bottom_text";
        private const String INSTANCE_INNER_BOTTOM_TEXT_COLOR = "inner_bottom_text_color";
        private const String INSTANCE_FINISHED_STROKE_COLOR = "finished_stroke_color";
        private const String INSTANCE_UNFINISHED_STROKE_COLOR = "unfinished_stroke_color";
        private const String INSTANCE_MAX = "max";
        private const String INSTANCE_PROGRESS = "progress";
        private const String INSTANCE_SUFFIX = "suffix";
        private const String INSTANCE_PREFIX = "prefix";
        private const String INSTANCE_FINISHED_STROKE_WIDTH = "finished_stroke_width";
        private const String INSTANCE_UNFINISHED_STROKE_WIDTH = "unfinished_stroke_width";
        private const String INSTANCE_BACKGROUND_COLOR = "inner_background_color";
        private const String INSTANCE_STARTING_DEGREE = "starting_degree";
        private const String INSTANCE_INNER_DRAWABLE = "inner_drawable";






        protected void initPainters()
        {
            if (showText)
            {
                _textPaint = new TextPaint();
                _textPaint.Color = textColor;
                _textPaint.TextSize = textSize;
                _textPaint.AntiAlias = true;

                _innerBottomTextPaint = new TextPaint();
                _innerBottomTextPaint.Color = innerBottomTextColor;
                _innerBottomTextPaint.TextSize = innerBottomTextSize;
                _innerBottomTextPaint.AntiAlias = true;
            }

            _finishedPaint = new Paint();
            _finishedPaint.Color = finishedStrokeColor;
            _finishedPaint.SetStyle(Paint.Style.Stroke);
            _finishedPaint.AntiAlias = true;
            _finishedPaint.StrokeWidth = finishedStrokeWidth;

            _unfinishedPaint = new Paint();
            _unfinishedPaint.Color = unfinishedStrokeColor;
            _unfinishedPaint.SetStyle(Paint.Style.Stroke);
            _unfinishedPaint.AntiAlias = true;
            _unfinishedPaint.StrokeWidth = unfinishedStrokeWidth;

            _innerCirclePaint = new Paint();
            _innerCirclePaint.Color = innerBackgroundColor;
            _innerCirclePaint.AntiAlias = true;
        }




        protected void initByAttributes(TypedArray attributes)
        {
            finishedStrokeColor = attributes.GetColor(Resource.Styleable.DonutProgress_donut_finished_color, default_finished_color);
            unfinishedStrokeColor = attributes.GetColor(Resource.Styleable.DonutProgress_donut_unfinished_color, default_unfinished_color);
            showText = attributes.GetBoolean(Resource.Styleable.DonutProgress_donut_show_text, true);
            attributeResourceId = attributes.GetResourceId(Resource.Styleable.DonutProgress_donut_inner_drawable, 0);

            setMax(attributes.GetInt(Resource.Styleable.DonutProgress_donut_max, default_max));
            setProgress(attributes.GetFloat(Resource.Styleable.DonutProgress_donut_progress, 0));
            finishedStrokeWidth = attributes.GetDimension(Resource.Styleable.DonutProgress_donut_finished_stroke_width, default_stroke_width);
            unfinishedStrokeWidth = attributes.GetDimension(Resource.Styleable.DonutProgress_donut_unfinished_stroke_width, default_stroke_width);

            if (showText)
            {
                if (attributes.GetString(Resource.Styleable.DonutProgress_donut_prefix_text) != null)
                {
                    prefixText = attributes.GetString(Resource.Styleable.DonutProgress_donut_prefix_text);
                }
                if (attributes.GetString(Resource.Styleable.DonutProgress_donut_suffix_text) != null)
                {
                    suffixText = attributes.GetString(Resource.Styleable.DonutProgress_donut_suffix_text);
                }
                if (attributes.GetString(Resource.Styleable.DonutProgress_donut_text) != null)
                {
                    text = attributes.GetString(Resource.Styleable.DonutProgress_donut_text);
                }

                textColor = attributes.GetColor(Resource.Styleable.DonutProgress_donut_text_color, default_text_color);
                textSize = attributes.GetDimension(Resource.Styleable.DonutProgress_donut_text_size, default_text_size);
                innerBottomTextSize = attributes.GetDimension(Resource.Styleable.DonutProgress_donut_inner_bottom_text_size, default_inner_bottom_text_size);
                innerBottomTextColor = attributes.GetColor(Resource.Styleable.DonutProgress_donut_inner_bottom_text_color, default_inner_bottom_text_color);
                innerBottomText = attributes.GetString(Resource.Styleable.DonutProgress_donut_inner_bottom_text);
            }

            innerBottomTextSize = attributes.GetDimension(Resource.Styleable.DonutProgress_donut_inner_bottom_text_size, default_inner_bottom_text_size);
            innerBottomTextColor = attributes.GetColor(Resource.Styleable.DonutProgress_donut_inner_bottom_text_color, default_inner_bottom_text_color);
            innerBottomText = attributes.GetString(Resource.Styleable.DonutProgress_donut_inner_bottom_text);

            startingDegree = attributes.GetInt(Resource.Styleable.DonutProgress_donut_circle_starting_degree, default_startingDegree);
            innerBackgroundColor = attributes.GetColor(Resource.Styleable.DonutProgress_donut_background_color, default_inner_background_color);
        }



        public override void Invalidate()
        {
            initPainters();
            base.Invalidate();
        }



        #region java props :D 

        public bool isShowText()
        {
            return showText;
        }

        public void setShowText(bool showText)
        {
            this.showText = showText;
        }

        public float getFinishedStrokeWidth()
        {
            return finishedStrokeWidth;
        }

        public void setFinishedStrokeWidth(float finishedStrokeWidth)
        {
            this.finishedStrokeWidth = finishedStrokeWidth;
            this.Invalidate();
        }

        public float getUnfinishedStrokeWidth()
        {
            return unfinishedStrokeWidth;
        }

        public void setUnfinishedStrokeWidth(float unfinishedStrokeWidth)
        {
            this.unfinishedStrokeWidth = unfinishedStrokeWidth;
            this.Invalidate();
        }

        private float getProgressAngle()
        {
            return getProgress() / (float)max * 360f;
        }

        public float getProgress()
        {
            return progress;
        }


        [Java.Interop.Export("setProgress")]
        public void setProgress(float progress)
        {
            this.progress = progress;
            if (this.progress > getMax())
            {
                this.progress %= getMax();
            }
            Invalidate();
        }

        public int getMax()
        {
            return max;
        }

        public void setMax(int max)
        {
            if (max > 0)
            {
                this.max = max;
                Invalidate();
            }
        }

        public float getTextSize()
        {
            return textSize;
        }

        public void setTextSize(float textSize)
        {
            this.textSize = textSize;
            this.Invalidate();
        }

        public int getTextColor()
        {
            return textColor;
        }

        public void setTextColor(Color textColor)
        {
            this.textColor = textColor;
            this.Invalidate();
        }

        public int getFinishedStrokeColor()
        {
            return finishedStrokeColor;
        }

        public void setFinishedStrokeColor(Color finishedStrokeColor)
        {
            this.finishedStrokeColor = finishedStrokeColor;
            this.Invalidate();
        }

        public int getUnfinishedStrokeColor()
        {
            return unfinishedStrokeColor;
        }

        public void setUnfinishedStrokeColor(Color unfinishedStrokeColor)
        {
            this.unfinishedStrokeColor = unfinishedStrokeColor;
            this.Invalidate();
        }

        public String getText()
        {
            return text;
        }

        public void setText(String text)
        {
            this.text = text;
            this.Invalidate();
        }

        public String getSuffixText()
        {
            return suffixText;
        }

        public void setSuffixText(String suffixText)
        {
            this.suffixText = suffixText;
            this.Invalidate();
        }

        public String getPrefixText()
        {
            return prefixText;
        }

        public void setPrefixText(String prefixText)
        {
            this.prefixText = prefixText;
            this.Invalidate();
        }

        public int getInnerBackgroundColor()
        {
            return innerBackgroundColor;
        }

        public void setInnerBackgroundColor(Color innerBackgroundColor)
        {
            this.innerBackgroundColor = innerBackgroundColor;
            this.Invalidate();
        }


        public String getInnerBottomText()
        {
            return innerBottomText;
        }

        public void setInnerBottomText(String innerBottomText)
        {
            this.innerBottomText = innerBottomText;
            this.Invalidate();
        }


        public float getInnerBottomTextSize()
        {
            return innerBottomTextSize;
        }

        public void setInnerBottomTextSize(float innerBottomTextSize)
        {
            this.innerBottomTextSize = innerBottomTextSize;
            this.Invalidate();
        }

        public int getInnerBottomTextColor()
        {
            return innerBottomTextColor;
        }

        public void setInnerBottomTextColor(Color innerBottomTextColor)
        {
            this.innerBottomTextColor = innerBottomTextColor;
            this.Invalidate();
        }

        public int getStartingDegree()
        {
            return startingDegree;
        }

        public void setStartingDegree(int startingDegree)
        {
            this.startingDegree = startingDegree;
            this.Invalidate();
        }

        public int getAttributeResourceId()
        {
            return attributeResourceId;
        }

        public void setAttributeResourceId(int attributeResourceId)
        {
            this.attributeResourceId = attributeResourceId;
        }

        #endregion


        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            SetMeasuredDimension(measure(widthMeasureSpec), measure(heightMeasureSpec));

            //TODO calculate inner circle height and then position bottom text at the bottom (3/4)
            innerBottomTextHeight = Height - (Height * 3) / 4;
        }

        private int measure(int measureSpec)
        {
            int result;
            var mode = MeasureSpec.GetMode(measureSpec);
            int size = MeasureSpec.GetSize(measureSpec);
            if (mode == MeasureSpecMode.Exactly)
            {
                result = size;
            }
            else
            {
                result = min_size;
                if (mode == MeasureSpecMode.AtMost)
                {
                    result = Math.Min(result, size);
                }
            }
            return result;
        }



        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            float delta = Math.Max(finishedStrokeWidth, unfinishedStrokeWidth);
            _finishedOuterRect.Set(delta,
                    delta,
                    Width - delta,
                    Height - delta);
            _unfinishedOuterRect.Set(delta,
                    delta,
                    Width - delta,
                    Height - delta);

            float innerCircleRadius = (Width - Math.Min(finishedStrokeWidth, unfinishedStrokeWidth) + Math.Abs(finishedStrokeWidth - unfinishedStrokeWidth)) / 2f;
            canvas.DrawCircle(Width / 2.0f, Height / 2.0f, innerCircleRadius, _innerCirclePaint);
            canvas.DrawArc(_finishedOuterRect, getStartingDegree(), getProgressAngle(), false, _finishedPaint);
            canvas.DrawArc(_unfinishedOuterRect, getStartingDegree() + getProgressAngle(), 360 - getProgressAngle(), false, _unfinishedPaint);

            if (showText)
            {
                String text = this.text != null ? this.text : prefixText + progress + suffixText;
                if (!TextUtils.IsEmpty(text))
                {
                    float textHeight = _textPaint.Descent() + _textPaint.Ascent();
                    canvas.DrawText(text, (Width - _textPaint.MeasureText(text)) / 2.0f, (Width - textHeight) / 2.0f, _textPaint);
                }

                if (!TextUtils.IsEmpty(getInnerBottomText()))
                {
                    _innerBottomTextPaint.TextSize = innerBottomTextSize;
                    float bottomTextBaseline = Height - innerBottomTextHeight - (_textPaint.Descent() + _textPaint.Ascent()) / 2;
                    canvas.DrawText(getInnerBottomText(), (Width - _innerBottomTextPaint.MeasureText(getInnerBottomText())) / 2.0f, bottomTextBaseline, _innerBottomTextPaint);
                }
            }

            if (attributeResourceId != 0)
            {
                Bitmap bitmap = BitmapFactory.DecodeResource(Resources, attributeResourceId);
                canvas.DrawBitmap(bitmap, (Width - bitmap.Width) / 2.0f, (Height - bitmap.Height) / 2.0f, null);
            }
        }



        protected override IParcelable OnSaveInstanceState()
        {
            Bundle bundle = new Bundle();
            bundle.PutParcelable(INSTANCE_STATE, base.OnSaveInstanceState());
            bundle.PutInt(INSTANCE_TEXT_COLOR, getTextColor());
            bundle.PutFloat(INSTANCE_TEXT_SIZE, getTextSize());
            bundle.PutFloat(INSTANCE_INNER_BOTTOM_TEXT_SIZE, getInnerBottomTextSize());
            bundle.PutFloat(INSTANCE_INNER_BOTTOM_TEXT_COLOR, getInnerBottomTextColor());
            bundle.PutString(INSTANCE_INNER_BOTTOM_TEXT, getInnerBottomText());
            bundle.PutInt(INSTANCE_INNER_BOTTOM_TEXT_COLOR, getInnerBottomTextColor());
            bundle.PutInt(INSTANCE_FINISHED_STROKE_COLOR, getFinishedStrokeColor());
            bundle.PutInt(INSTANCE_UNFINISHED_STROKE_COLOR, getUnfinishedStrokeColor());
            bundle.PutInt(INSTANCE_MAX, getMax());
            bundle.PutInt(INSTANCE_STARTING_DEGREE, getStartingDegree());
            bundle.PutFloat(INSTANCE_PROGRESS, getProgress());
            bundle.PutString(INSTANCE_SUFFIX, getSuffixText());
            bundle.PutString(INSTANCE_PREFIX, getPrefixText());
            bundle.PutString(INSTANCE_TEXT, getText());
            bundle.PutFloat(INSTANCE_FINISHED_STROKE_WIDTH, getFinishedStrokeWidth());
            bundle.PutFloat(INSTANCE_UNFINISHED_STROKE_WIDTH, getUnfinishedStrokeWidth());
            bundle.PutInt(INSTANCE_BACKGROUND_COLOR, getInnerBackgroundColor());
            bundle.PutInt(INSTANCE_INNER_DRAWABLE, getAttributeResourceId());
            return bundle;
        }


        protected override void OnRestoreInstanceState(IParcelable state)
        {
            if (state is Bundle bundle)
            {
                textColor = new Color(bundle.GetInt(INSTANCE_TEXT_COLOR));
                textSize = bundle.GetFloat(INSTANCE_TEXT_SIZE);
                innerBottomTextSize = bundle.GetFloat(INSTANCE_INNER_BOTTOM_TEXT_SIZE);
                innerBottomText = bundle.GetString(INSTANCE_INNER_BOTTOM_TEXT);
                innerBottomTextColor = new Color(bundle.GetInt(INSTANCE_INNER_BOTTOM_TEXT_COLOR));
                finishedStrokeColor = new Color(bundle.GetInt(INSTANCE_FINISHED_STROKE_COLOR));
                unfinishedStrokeColor = new Color(bundle.GetInt(INSTANCE_UNFINISHED_STROKE_COLOR));
                finishedStrokeWidth = bundle.GetFloat(INSTANCE_FINISHED_STROKE_WIDTH);
                unfinishedStrokeWidth = bundle.GetFloat(INSTANCE_UNFINISHED_STROKE_WIDTH);
                innerBackgroundColor = new Color(bundle.GetInt(INSTANCE_BACKGROUND_COLOR));
                attributeResourceId = bundle.GetInt(INSTANCE_INNER_DRAWABLE);
                initPainters();
                setMax(bundle.GetInt(INSTANCE_MAX));
                setStartingDegree(bundle.GetInt(INSTANCE_STARTING_DEGREE));
                setProgress(bundle.GetFloat(INSTANCE_PROGRESS));
                prefixText = bundle.GetString(INSTANCE_PREFIX);
                suffixText = bundle.GetString(INSTANCE_SUFFIX);
                text = bundle.GetString(INSTANCE_TEXT);
                base.OnRestoreInstanceState((IParcelable)bundle.GetParcelable(INSTANCE_STATE));
                return;
            }
            base.OnRestoreInstanceState(state);
        }


        public void setDonut_progress(float percent)
        {
            setProgress(percent);
        }

        ObjectAnimator animator;
        public void AnimateCircular(float toValue)
        {
            if (animator != null)
                animator.Cancel();
            animator = ObjectAnimator.OfFloat(this, "Progress", 0.0f, toValue);
            animator.SetInterpolator(new CustInterpolator());
            animator.SetDuration(500);
            animator.RepeatMode = ValueAnimatorRepeatMode.Restart;
            animator.Start();
        }

        public float ValueProperty
        {
            get => getProgress();
            set => setProgress(value);
        }


        public float dp2px(Resources resources, float dp)
        {
            var scale = resources.DisplayMetrics.Density;
            return dp * scale + 0.5f;
        }

        public float sp2px(Resources resources, float sp)
        {
            var scale = resources.DisplayMetrics.ScaledDensity;
            return sp * scale;
        }
    }

    class CustInterpolator : Java.Lang.Object, ITimeInterpolator
    {
        public float GetInterpolation(float input)
        {
            return input * input;
        }
    }
}
