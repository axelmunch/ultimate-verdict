import {
  createContext,
  useContext,
  useRef,
  useState,
  useEffect,
  type ReactNode,
} from "react";

type TimeContextType = {
  currentTime: number;
  displayTimer: (
    startTime: number,
    endTime?: number,
    subsecondsDigits?: number,
  ) => string;
  getDate: (timestamp: number) => string;
};

const TimeContext = createContext<TimeContextType | undefined>(undefined);

function TimeProvider({ children }: { children: ReactNode }) {
  const [currentTime, setCurrentTime] = useState(Date.now());

  const frameRef = useRef(0);

  useEffect(() => {
    const update = async () => {
      setCurrentTime(Date.now());

      await new Promise((resolve) => setTimeout(resolve, 1000 / 2));

      frameRef.current = requestAnimationFrame(update);
    };

    frameRef.current = requestAnimationFrame(update);
    return () => cancelAnimationFrame(frameRef.current);
  }, []);

  const displayTimer = (
    startTime: number,
    endTime?: number,
    subsecondsDigits?: number,
  ): string => {
    const SUBSECONDS_DIGITS = subsecondsDigits ?? 2;
    const diff = (endTime ?? currentTime) - startTime;
    const hours = Math.floor(diff / 3600000);
    const minutes = Math.floor((diff % 3600000) / 60000);
    const seconds = Math.floor((diff % 60000) / 1000);
    const subseconds = Math.floor(
      (diff * 10 ** (SUBSECONDS_DIGITS - 3)) % 10 ** SUBSECONDS_DIGITS,
    );
    return (
      (hours > 0 ? `${String(hours).padStart(2, "0")}:` : "") +
      `${String(minutes).padStart(2, "0")}:${String(seconds).padStart(
        2,
        "0",
      )}` +
      (SUBSECONDS_DIGITS > 0
        ? `:${String(subseconds).padStart(SUBSECONDS_DIGITS, "0")}`
        : "")
    );
  };

  const getDate = (timestamp: number): string => {
    const date = new Date(timestamp);

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");

    return `${year}-${month}-${day}`;
  };

  return (
    <TimeContext.Provider value={{ currentTime, displayTimer, getDate }}>
      {children}
    </TimeContext.Provider>
  );
}

export const useTime = (): TimeContextType => {
  const context = useContext(TimeContext);
  if (!context) {
    throw new Error(`useTime must be used within a ${TimeProvider.name}`);
  }
  return context;
};

export default TimeProvider;
