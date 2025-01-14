(ns wheel.offset-date-time
  (:require [clojure.spec.alpha :as s])
  (:import [java.time.format DateTimeFormatter DateTimeParseException]
           [java.time OffsetDateTime ZoneId]))

(defn iso-8061-format? [x]
  (try
    (.parse DateTimeFormatter/ISO_OFFSET_DATE_TIME x)
    true
    (catch DateTimeParseException e
      false)))

(defn ist? [x]
  (if (iso-8061-format? x)
    (= (.. (OffsetDateTime/parse x) getOffset toString)
       "+05:30")
    false))

(defn ist-now []
  (OffsetDateTime/now (ZoneId/of "+05:30")))

(s/def ::iso-8061-format (s/and string? iso-8061-format?))
(s/def ::ist-timestamp (s/and ::iso-8061-format ist?))

(comment
  (str (ist-now))
  (s/valid? ::ist-timestamp "2007-04-05T12:30-02:00")
  (s/valid? ::ist-timestamp "2019-10-01T06:56+05:30"))
