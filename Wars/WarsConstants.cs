
public class Constants
{
    public static float MAX_SPEED = 35f; // 35f
    public static float ENEMY_MAX_SPEED = 40f; // 40f
    public static float THROTTLE_POWER = 10f; // 10f
    public static float ENEMY_THROTTLE_POWER = 12f; // 12f

    public static float SMALLEST_ANGLE_DIFF_ACCEPT = 0.04f; // 0.04f

    public static float REDSHOT_TTL = 5f; // 5f
    public static float REDSHOT_SPACING = 0.3f;//3f;//0.3f; // seconds
    public static float ENEMY_REDSHOT_SPACING = 3f; // seconds
    public static float REDSHOT_SPEED = 20f; // 20f

    public static int   EXPLOSION_DEBRIS_COUNT = 200;
    public static float EXPLOSION_DEBRIS_TTL = 1f;
    public static float EXPLOSION_DEBRIS_TTL_VARIANCE = 0.9f; // should be < EXPLOSION_DEBRIS_TTL
    public static float EXPLOSION_DEBRIS_SPEED = 7f;
    public static float EXPLOSION_DEBRIS_SPEED_VARIANCE = 4f; // should be < EXPLOSION_DEBRIS_SPEED

    public static float NUMBER_OF_STARS = 100; // 200
    public static int FUME_COUNT = 8; // 8
    public static float FUME_TTL = 0.4f; // 0.4 (seconds)
    public static float FUME_SPEED_FACTOR = 10f; // 6
    public static float FUME_SPEED_AFFECT_FACTOR = 6f; // lower value makes fumes more affected by ship speed

    public static int SHIP_TYPE_MANUALLY_CONTROLLED = 0;
    public static int SHIP_TYPE_COMPUTER_CONTROLLED = 1;
    public static int SHIP_TYPE_CAT = 2;

    public static bool DO_FLASH = false; // true

    public static int RESOLUTION_HEIGHT_DEBUG = 600; // 600, 1050, 1000
    public static int RESOLUTION_WIDTH_DEBUG = (int)(((float)RESOLUTION_HEIGHT_DEBUG) * 1.6f); // 960, 1680, 1600
    public static bool RESOLUTION_FULLSCREEN_DEBUG = false;

    public static int RESOLUTION_WIDTH_RELEASE = 1920;//1680; //1920;
    public static int RESOLUTION_HEIGHT_RELEASE = 1200;//1050; //1200;
    public static bool RESOLUTION_FULLSCREEN_RELEASE = true;

#if DEBUG
    public static int  RESOLUTION_WIDTH      = RESOLUTION_WIDTH_DEBUG;
    public static int  RESOLUTION_HEIGHT     = RESOLUTION_HEIGHT_DEBUG;
    public static bool RESOLUTION_FULLSCREEN = RESOLUTION_FULLSCREEN_DEBUG;
#else
    public static int RESOLUTION_WIDTH = RESOLUTION_WIDTH_RELEASE;
    public static int RESOLUTION_HEIGHT = RESOLUTION_HEIGHT_RELEASE;
    public static bool RESOLUTION_FULLSCREEN = RESOLUTION_FULLSCREEN_RELEASE;
#endif
}
