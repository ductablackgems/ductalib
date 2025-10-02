using _0.DucTALib.Scripts.Common;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _0.DucTALib.Splash
{
    public class ContentCarousel: MonoBehaviour, IEndDragHandler, IBeginDragHandler
    {
        public enum LayoutType
        {
            Vertical,
            Horizontal
        }

        [Title("Layout Settings")] [Tooltip("Select the layout type for the content carousel.")]
        public LayoutType layoutType = LayoutType.Vertical;

        [BoxGroup("Viewport Settings")] [Tooltip("Size of each page.")]
        public float pageSize = 600f;

        [BoxGroup("Viewport Settings")] [Tooltip("Swipe sensitivity.")]
        public float swipeThreshold = 0.2f;

        [BoxGroup("Viewport Settings")] [Tooltip("Speed of snap to target.")]
        public float snapSpeed = 8f;

        [BoxGroup("Navigation Buttons")] public Button nextButton;


        [BoxGroup("Carousel Mode")] public bool carouselMode = false;

        [BoxGroup("Carousel Mode")] [ShowIf("carouselMode")]
        public bool autoMove = false;

        [BoxGroup("Carousel Mode")] [ShowIf("autoMove")]
        public float autoMoveTimer = 5f;

        [BoxGroup("Navigation Dots")] public GameObject dotPrefab;

        [BoxGroup("Navigation Dots")] public GameObject dotsContainer;

        [BoxGroup("Dot Appearance")] public Color activeDotColor = Color.yellow;

        [BoxGroup("Dot Appearance")] public Color inactiveDotColor = Color.grey;

        [BoxGroup("Dot Appearance")] public float dotColorTransitionSpeed = 5f;

        [BoxGroup("Dot Size")] public Vector2 activeDotSize = new Vector2(20f, 10f);

        [BoxGroup("Dot Size")] public Vector2 inactiveDotSize = new Vector2(10f, 10f);

        [BoxGroup("Dot Size")] public float dotScalingSpeed = 5f;

        [BoxGroup("3D Rotation")] public float maxRotationAngle = 45f;

        [BoxGroup("3D Rotation")] public float rotationSpeed = 5f;

        [FoldoutGroup("Experimental Features")]
        public bool infiniteLooping = true;

        [ReadOnly, FoldoutGroup("Runtime Info")]
        public int totalPages;

        [ReadOnly, FoldoutGroup("Runtime Info")]
        public int currentIndex = 0;

        [FoldoutGroup("Runtime Info")] [Required]
        public GridLayoutGroup gridLayoutGroup;

        private ScrollRect scrollRect;
        private RectTransform contentRectTransform;
        private float targetPosition;
        private bool isDragging = false;
        private Vector2 dragStartPos;
        private float lastDragTime;
        private float autoMoveTimerCountdown;
        private void Start()
        {
            scrollRect = GetComponent<ScrollRect>();
            gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();

            if (gridLayoutGroup == null)
            {
                Debug.LogError("GridLayoutGroup not found in children. Make sure it is present.");
                return;
            }

            contentRectTransform = gridLayoutGroup.transform as RectTransform;

            if (contentRectTransform == null)
            {
                Debug.LogError("RectTransform not found on the GridLayoutGroup. Make sure it is present.");
                return;
            }

            gridLayoutGroup.startAxis = (layoutType == LayoutType.Vertical)
                ? GridLayoutGroup.Axis.Vertical
                : GridLayoutGroup.Axis.Horizontal;

            CalculateTotalPages();
            SetSnapTarget(0);

            if (carouselMode)
            {
                InitializeNavigationDots();
            }

            if (nextButton != null)
            {
                nextButton.onClick.AddListener(MoveToNextPage);
            }
        }

        private void InitializeNavigationDots()
        {
#if USE_ADMOB_NATIVE
             var total = CommonRemoteConfig.instance.splashConfig.introConfig.tutorialCount;
            for (int i = 0; i < total; i++)
            {
                GameObject dot = Instantiate(dotPrefab, dotsContainer.transform);
                SetDotSize(dot, i == currentIndex ? activeDotSize : inactiveDotSize);
                SetDotColor(dot, i == currentIndex ? activeDotColor : inactiveDotColor);
            }
#endif
           
        }

        private void SetDotColor(GameObject dot, Color color)
        {
            Image dotImage = dot.GetComponent<Image>();
            if (dotImage != null)
            {
                dotImage.color = color;
            }
        }

        private void SetDotSize(GameObject dot, Vector2 size)
        {
            RectTransform dotRect = dot.GetComponent<RectTransform>();
            if (dotRect != null)
            {
                dotRect.sizeDelta = size;
            }
        }

        private void CalculateTotalPages()
        {
            int itemCount = gridLayoutGroup.transform.childCount;
            totalPages = Mathf.CeilToInt((float)itemCount / gridLayoutGroup.constraintCount);
        }

        private void SetSnapTarget(int page)
        {
            if (infiniteLooping)
            {
                int totalVisiblePages = totalPages * 2; // Duplicate the pages to allow for looping
                int offsetPage = (page + totalVisiblePages) % totalPages;
                targetPosition = -pageSize * offsetPage;
            }
            else
            {
                targetPosition = -pageSize * page;
            }

            currentIndex = page;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
            dragStartPos = eventData.position;
            lastDragTime = Time.unscaledTime;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;

            float dragDistance = Mathf.Abs(eventData.position.x - dragStartPos.x);
            float dragSpeed = eventData.delta.x / (Time.unscaledTime - lastDragTime);

            if (autoMove)
            {
                autoMoveTimerCountdown = autoMoveTimer;
            }

            if (carouselMode)
            {
                if (dragDistance > pageSize * swipeThreshold || Mathf.Abs(dragSpeed) > swipeThreshold)
                {
                    int currentPage = Mathf.RoundToInt(contentRectTransform.anchoredPosition.x / -pageSize);

                    if (dragSpeed > 0)
                    {
                        MoveToPreviousPage();
                    }
                    else
                    {
                        MoveToNextPage();
                    }
                }
                else
                {
                    SetSnapTarget(currentIndex);
                }
            }
        }

        private void Update()
        {
            if (autoMove)
            {
                autoMoveTimerCountdown -= Time.deltaTime;
                if (autoMoveTimerCountdown <= 0f)
                {
                    MoveToNextPage();
                    autoMoveTimerCountdown = autoMoveTimer;
                }
            }

            if (!isDragging)
            {
                contentRectTransform.anchoredPosition = Vector2.Lerp(
                    contentRectTransform.anchoredPosition,
                    new Vector2(targetPosition, contentRectTransform.anchoredPosition.y),
                    Time.deltaTime * snapSpeed
                );

                UpdateDotSizes();

                RotateContent();
            }
        }

        private void RotateContent()
        {
            for (int i = 0; i < totalPages; i++)
            {
                GameObject content = gridLayoutGroup.transform.GetChild(i).gameObject;
                float rotationAngle = Mathf.Lerp(0f, maxRotationAngle, Mathf.Abs(currentIndex - i) / (float)totalPages);
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, rotationAngle);

                content.transform.rotation = Quaternion.Slerp(content.transform.rotation, targetRotation,
                    Time.deltaTime * rotationSpeed);
            }
        }

        private void MoveToPreviousPage()
        {
            if (infiniteLooping)
            {
                int prevPage = (currentIndex - 1 + totalPages) % totalPages;
                SetSnapTarget(prevPage);
            }
            else
            {
                int prevPage = Mathf.Clamp(currentIndex - 1, 0, totalPages - 1);
                SetSnapTarget(prevPage);
            }
        }

        public void MoveToNextPage()
        {
            if (infiniteLooping)
            {
                int nextPage = (currentIndex + 1) % totalPages;
                SetSnapTarget(nextPage);
            }
            else
            {
                int nextPage = Mathf.Clamp(currentIndex + 1, 0, totalPages - 1);
                SetSnapTarget(nextPage);
            }
        }

        private void UpdateDotSizes()
        {
#if  USE_ADMOB_NATIVE
             var count = CommonRemoteConfig.instance.splashConfig.introConfig.tutorialCount;
            for (int i = 0; i < count; i++)
            {
                GameObject dot = dotsContainer.transform.GetChild(i).gameObject;
                Vector2 targetSize = i == currentIndex ? activeDotSize : inactiveDotSize;
                RectTransform dotRect = dot.GetComponent<RectTransform>();

                if (dotRect != null)
                {
                    dotRect.sizeDelta = Vector2.Lerp(dotRect.sizeDelta, targetSize, Time.deltaTime * dotScalingSpeed);
                }

                Image dotImage = dot.GetComponent<Image>();
                if (dotImage != null)
                {
                    Color targetColor = i == currentIndex ? activeDotColor : inactiveDotColor;
                    dotImage.color = Color.Lerp(dotImage.color, targetColor, Time.deltaTime * dotColorTransitionSpeed);
                }
            }
#endif
           
        }
    }
}