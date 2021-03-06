﻿/* Projecto 2014
 * Faculdade Ciencias Universidade do Porto
 * 
 * Developed By Luis Do Nascimento
 * featuring codes from openslam.org and Edgard Quirino and Mário Almeida 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisualOdometry
{
	public class RobotPath
	{
		private List<Pose> m_Poses;
		private IList<Pose> m_ReadOnlyPoses;
		private List<DateTime> m_UtcTimeStamps;
		private IList<DateTime> m_ReadOnlyUtcTimeStamps;
		private double m_PathLength;

		private double m_MinX, m_MaxX, m_MinY, m_MaxY;

		public RobotPath()
		{
			m_Poses = new List<Pose>();
			m_ReadOnlyPoses = m_Poses.AsReadOnly();

			m_UtcTimeStamps = new List<DateTime>();
			m_ReadOnlyUtcTimeStamps = m_UtcTimeStamps.AsReadOnly();
		}

		public void Add(Pose currentPose)
		{
			Add(DateTime.UtcNow, currentPose);
		}

		public void Add(DateTime timeStamp, Pose pose)
		{
			m_UtcTimeStamps.Add(timeStamp.ToUniversalTime());
			m_Poses.Add(pose);

			if (pose.X < m_MinX)
			{
				m_MinX = pose.X;
			}
			if (pose.X > m_MaxX)
			{
				m_MaxX = pose.X;
			}

			if (pose.Y < m_MinY)
			{
				m_MinY = pose.Y;
			}
			if (pose.Y > m_MaxY)
			{
				m_MaxY = pose.Y;
			}

			if (m_Poses.Count > 1)
			{
				double dx = m_Poses[m_Poses.Count - 2].X - pose.X;
				double dy = m_Poses[m_Poses.Count - 2].Y - pose.Y;

				m_PathLength += Math.Sqrt(dx * dx + dy * dy);
			}
		}

		public IList<Pose> Poses
		{
			get { return m_ReadOnlyPoses; }
		}

		public IList<DateTime> UtcTimeStamps
		{
			get { return m_ReadOnlyUtcTimeStamps; }
		}

		public double MinX { get { return m_MinX; } }
		public double MaxX { get { return m_MaxX; } }
		public double MinY { get { return m_MinY; } }
		public double MaxY { get { return m_MaxY; } }

		public double PathLength { get { return m_PathLength; } }

		public double DistanceFromStart
		{
			get
			{
				if (m_Poses.Count < 2)
				{
					return 0;
				}

				double dx = m_Poses[m_Poses.Count - 1].X - m_Poses[0].X;
				double dy = m_Poses[m_Poses.Count - 1].Y - m_Poses[0].Y;
				return Math.Sqrt(dx * dx + dy * dy);
			}
		}

		public void Clear()
		{
			m_Poses.Clear();
			m_UtcTimeStamps.Clear();
			m_PathLength = 0;
			m_MinX = 0;
			m_MaxX = 0;
			m_MinY = 0;
			m_MaxY = 0;
		}
	}
}
