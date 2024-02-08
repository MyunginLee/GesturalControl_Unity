using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.Holistic
{
    public class HolisticTrackingSolution : ImageSourceSolution<HolisticTrackingGraph>
    {
        [SerializeField] private RectTransform _worldAnnotationArea;
        [SerializeField] private DetectionAnnotationController _poseDetectionAnnotationController;
        [SerializeField] private HolisticLandmarkListAnnotationController _holisticAnnotationController;
        [SerializeField] private PoseWorldLandmarkListAnnotationController _poseWorldLandmarksAnnotationController;
        [SerializeField] private MaskAnnotationController _segmentationMaskAnnotationController;
        [SerializeField] private NormalizedRectAnnotationController _poseRoiAnnotationController;
        public HolisticTrackingGraph.ModelComplexity modelComplexity
        {
            get => graphRunner.modelComplexity;
            set => graphRunner.modelComplexity = value;
        }

        public bool smoothLandmarks
        {
            get => graphRunner.smoothLandmarks;
            set => graphRunner.smoothLandmarks = value;
        }

        public bool refineFaceLandmarks
        {
            get => graphRunner.refineFaceLandmarks;
            set => graphRunner.refineFaceLandmarks = value;
        }

        public bool enableSegmentation
        {
            get => graphRunner.enableSegmentation;
            set => graphRunner.enableSegmentation = value;
        }

        public bool smoothSegmentation
        {
            get => graphRunner.smoothSegmentation;
            set => graphRunner.smoothSegmentation = value;
        }

        public float minDetectionConfidence
        {
            get => graphRunner.minDetectionConfidence;
            set => graphRunner.minDetectionConfidence = value;
        }

        public float minTrackingConfidence
        {
            get => graphRunner.minTrackingConfidence;
            set => graphRunner.minTrackingConfidence = value;
        }

        protected override void SetupScreen(ImageSource imageSource)
        {
            //      base.SetupScreen(imageSource);
            _worldAnnotationArea.localEulerAngles = imageSource.rotation.Reverse().GetEulerAngles();
        }

        protected override void OnStartRun()
        {
            if (!runningMode.IsSynchronous())
            {
                graphRunner.OnPoseDetectionOutput += OnPoseDetectionOutput;
                graphRunner.OnFaceLandmarksOutput += OnFaceLandmarksOutput;
                graphRunner.OnPoseLandmarksOutput += OnPoseLandmarksOutput;
                graphRunner.OnLeftHandLandmarksOutput += OnLeftHandLandmarksOutput;
                graphRunner.OnRightHandLandmarksOutput += OnRightHandLandmarksOutput;
                graphRunner.OnPoseWorldLandmarksOutput += OnPoseWorldLandmarksOutput;
                graphRunner.OnSegmentationMaskOutput += OnSegmentationMaskOutput;
                graphRunner.OnPoseRoiOutput += OnPoseRoiOutput;
            }

            var imageSource = ImageSourceProvider.ImageSource;
            SetupAnnotationController(_poseDetectionAnnotationController, imageSource);
            SetupAnnotationController(_holisticAnnotationController, imageSource);
            SetupAnnotationController(_poseWorldLandmarksAnnotationController, imageSource);
            SetupAnnotationController(_segmentationMaskAnnotationController, imageSource);
            _segmentationMaskAnnotationController.InitScreen(imageSource.textureWidth, imageSource.textureHeight);
            SetupAnnotationController(_poseRoiAnnotationController, imageSource);
        }

        protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
        {
            graphRunner.AddTextureFrameToInputStream(textureFrame);
        }

        protected override IEnumerator WaitForNextValue()
        {
            Detection poseDetection = null;
            NormalizedLandmarkList faceLandmarks = null;
            NormalizedLandmarkList poseLandmarks = null;
            NormalizedLandmarkList leftHandLandmarks = null;
            NormalizedLandmarkList rightHandLandmarks = null;
            LandmarkList poseWorldLandmarks = null;
            ImageFrame segmentationMask = null;
            NormalizedRect poseRoi = null;
            if (runningMode == RunningMode.Sync)
            {
                var _ = graphRunner.TryGetNext(out poseDetection, out poseLandmarks, out faceLandmarks, out leftHandLandmarks, out rightHandLandmarks, out poseWorldLandmarks, out segmentationMask, out poseRoi, true);
            }
            else if (runningMode == RunningMode.NonBlockingSync)
            {
                yield return new WaitUntil(() =>
                  graphRunner.TryGetNext(out poseDetection, out poseLandmarks, out faceLandmarks, out leftHandLandmarks, out rightHandLandmarks, out poseWorldLandmarks, out segmentationMask, out poseRoi, false));
            }
            _poseDetectionAnnotationController.DrawNow(poseDetection);
            _holisticAnnotationController.DrawNow(faceLandmarks, poseLandmarks, leftHandLandmarks, rightHandLandmarks);
            _poseWorldLandmarksAnnotationController.DrawNow(poseWorldLandmarks);
            _segmentationMaskAnnotationController.DrawNow(segmentationMask);
            _poseRoiAnnotationController.DrawNow(poseRoi);
        }

        private void OnPoseDetectionOutput(object stream, OutputEventArgs<Detection> eventArgs)
        {
            _poseDetectionAnnotationController.DrawLater(eventArgs.value);
        }

        public void OnFaceLandmarksOutput(object stream, OutputEventArgs<NormalizedLandmarkList> eventArgs)
        {
            _holisticAnnotationController.DrawFaceLandmarkListLater(eventArgs.value);
        }

        private void OnPoseLandmarksOutput(object stream, OutputEventArgs<NormalizedLandmarkList> eventArgs)
        {
            _holisticAnnotationController.DrawPoseLandmarkListLater(eventArgs.value);
            if (eventArgs.value != null)
            {
                for (int i = 0; i < 32; i++)
                {
                    Gesture.gen.pose[i].x = eventArgs.value.Landmark[i].X;
                    Gesture.gen.pose[i].y = eventArgs.value.Landmark[i].Y;
                    Gesture.gen.pose[i].z = eventArgs.value.Landmark[i].Z;
                }
            }
        }

        private void OnLeftHandLandmarksOutput(object stream, OutputEventArgs<NormalizedLandmarkList> eventArgs)
        {
            _holisticAnnotationController.DrawLeftHandLandmarkListLater(eventArgs.value);
            if (eventArgs.value != null)
            {
                for (int i = 0; i < 20; i++)
                {
                    Gesture.gen.lefthandpos[i].x = eventArgs.value.Landmark[i].X;
                    Gesture.gen.lefthandpos[i].y = eventArgs.value.Landmark[i].Y;
                    Gesture.gen.lefthandpos[i].z = eventArgs.value.Landmark[i].Z;
                }
            }
        }

        private void OnRightHandLandmarksOutput(object stream, OutputEventArgs<NormalizedLandmarkList> eventArgs)
        {
            _holisticAnnotationController.DrawRightHandLandmarkListLater(eventArgs.value);
            if (eventArgs.value != null)
            {
                for (int i = 0; i < 20; i++)
                {
                    Gesture.gen.righthandpos[i].x = eventArgs.value.Landmark[i].X;
                    Gesture.gen.righthandpos[i].y = eventArgs.value.Landmark[i].Y;
                    Gesture.gen.righthandpos[i].z = eventArgs.value.Landmark[i].Z;
                }
            }
        }

        private void OnPoseWorldLandmarksOutput(object stream, OutputEventArgs<LandmarkList> eventArgs)
        {
            //_poseworldlandmarksannotationcontroller.drawlater(eventargs.value);
            //if (eventArgs.value != null)
            //{

            //}
        }

        private void OnSegmentationMaskOutput(object stream, OutputEventArgs<ImageFrame> eventArgs)
        {
            _segmentationMaskAnnotationController.DrawLater(eventArgs.value);
        }

        private void OnPoseRoiOutput(object stream, OutputEventArgs<NormalizedRect> eventArgs)
        {
            _poseRoiAnnotationController.DrawLater(eventArgs.value);
        }
    }
}
